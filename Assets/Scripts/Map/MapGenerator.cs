#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Mirror;
[RequireComponent(typeof(MeshFilter))]
public class MapGenerator : NetworkBehaviour
{
	public int perlinScale = 30;
	public float heightOffsetStrength;
	public int perlinOffsetRandomness;

	[SerializeField] Texture2D mapTexture = default;
	[SerializeField] Material material = default;

	[SerializeField] Team[] teams = default;
	public ColorToObject[] colorObjectMappings;
	[SerializeField] ColorToHeight[] colorHeightMappings = default;

	Mesh waterMesh;
	public int gridSpacing { get; private set; } = 1;
	public int xSize { get; private set; }
	public int zSize { get; private set; }
	public Material waterMaterial = default;

	Mesh mesh;
	Vector3[] vertices;
	int[] triangles;
	Vector2[] uv;

	[Client]
	public void SetupMap()
	{
		SetDimension();
		waterMaterial.SetFloat("StartTime", -999);
		GenerateMap();
		BuildObjectsOnMap();
	}

	void SetDimension()
	{
		xSize = mapTexture.width - mapTexture.width % 2;
		zSize = mapTexture.height - mapTexture.height % 2;
	}

	private void OnDisable()
	{
		waterMaterial.SetFloat("StartTime", -999);
	}


	public void GenerateMap()
	{
		DestroyChildren();
		SetDimension();
		BuildWaterMesh();
		SetupMeshfilter();
		BuildMapMesh();
		ChangeMapVertexHeights();
		UpdateMesh();
		UpdateCollider();

		material.SetInt("Vector1_2D88299F", gridSpacing);
		material.SetTexture("Texture2D_AD5527E4", mapTexture);
	}



	private void DestroyChildren()
	{
		//Destroy old Placeables that were instantiated by previous mapgeneration
		//TODO Pool?
		if (Application.isEditor)
			for (int i = transform.childCount; i > 0; --i)
			{
				DestroyImmediate(this.transform.GetChild(0).gameObject);
			}
		else
		{
			for (int i = this.transform.childCount; i > 0; --i)
				Destroy(this.transform.GetChild(0).gameObject);
		}
	}

	private void UpdateCollider()
	{
		//set new Mesh for Meshcollider
		GetComponent<MeshCollider>().sharedMesh = mesh;
	}

	private void SetupMeshfilter()
	{
		//clear out previous mesh and setup new parameters
		mesh = new Mesh();
		MeshFilter filter = GetComponent<MeshFilter>();
		if (filter.sharedMesh != null)
			filter.sharedMesh.Clear();
		filter.sharedMesh = mesh;
		GetComponent<MeshRenderer>().material = material;
		mesh.name = "Map";
	}

	private void BuildMapMesh()
	{
		//initialize vertices and uv Arrays with Texture dimensions
		vertices = new Vector3[(xSize + 1) * (zSize + 1)];
		uv = new Vector2[vertices.Length];

		for (int i = 0, z = 0; z <= zSize * gridSpacing; z += gridSpacing)
		{
			for (int x = 0; x <= xSize * gridSpacing; x += gridSpacing, i++)
			{
				//set uv coordinates
				uv[i] = new Vector2((float)x / (xSize * gridSpacing), (float)z / (zSize * gridSpacing));
				//populate vertex array with slightly random height vertices
				//random offsets for less prdicatble result
				float randOffset = UnityEngine.Random.Range(-perlinOffsetRandomness, perlinOffsetRandomness);
				float yOffset = Mathf.PerlinNoise(uv[i].x * perlinScale + randOffset, uv[i].y * perlinScale + randOffset);
				vertices[i] = new Vector3(x, yOffset * heightOffsetStrength, z);

			}
		}

		//initialize triangles Array with Texture dimensions
		triangles = new int[xSize * zSize * 6];
		int vert = 0;
		int tris = 0;
		for (int z = 0; z < zSize; z++)
		{
			for (int x = 0; x < xSize; x++)
			{
				//populate triangles Array with vertex indices (in specific order for normals and rendering)
				triangles[tris + 0] = vert + 0;
				triangles[tris + 1] = vert + xSize + 1;
				triangles[tris + 2] = vert + 1;
				triangles[tris + 3] = vert + 1;
				triangles[tris + 4] = vert + xSize + 1;
				triangles[tris + 5] = vert + xSize + 2;

				vert++;
				tris += 6;
			}
			vert++;
		}
	}

	private void ChangeMapVertexHeights()
	{
		for (int i = 0, z = 0; z <= zSize * gridSpacing; z += gridSpacing)
		{
			for (int x = 0; x <= xSize * gridSpacing; x += gridSpacing, i++)
			{

				foreach (ColorToHeight heightMapping in colorHeightMappings)
				{
					//get Texturecolors at vertexposition as well as left, lower and lower left of position
					Color32 mapTextureColor = mapTexture.GetPixel(x, z);
					Color32 mapTextureColorLeftX = mapTexture.GetPixel(x - 1, z);
					Color32 mapTextureColorUpZ = mapTexture.GetPixel(x, z - 1);
					Color32 mapTextureColorLeftXUpZ = mapTexture.GetPixel(x - 1, z - 1);

					//check if left/lower texturepixel have same Value as this mapping
					if (heightMapping.value == mapTextureColor.r || heightMapping.value == mapTextureColorLeftX.r
						|| heightMapping.value == mapTextureColorUpZ.r || heightMapping.value == mapTextureColorLeftXUpZ.r)
					{
						//use this mappings saved height for this vertex, if any of the left/lower texturepixel have corresponding value
						vertices[i].y += heightMapping.vertexHight;
					}

				}
			}
		}

	}

	private void BuildObjectsOnMap()
	{
		for (int i = 0, z = 0; z <= zSize * gridSpacing; z += gridSpacing)
		{
			for (int x = 0; x <= xSize * gridSpacing; x += gridSpacing, i++)
			{
				//get textureColor at coordinates
				Color32 mapTextureColor = mapTexture.GetPixel(x, z);

				foreach (ColorToObject objectMapping in colorObjectMappings)
				{
					//check if texture has corresponding green-value as this mapping
					if (objectMapping.value == mapTextureColor.g)
					{
						if (objectMapping.placeable != null)
						{
							//ALSO USED IN PLACEMENTCONTROLLER
							float newY = vertices[i].y +
										 vertices[i + 1].y +
										 vertices[xSize + i + 1].y +
										 vertices[xSize + i].y;
							newY /= 4;
							//instantiate given prefab and set Position at coordinsates + gridspacing/2 offset and vertexheigth
							GameObject obj = Instantiate(objectMapping.placeable, transform) as GameObject;
							obj.transform.position = new Vector3(x + gridSpacing / 2.0f, newY, z + gridSpacing / 2.0f);
							NetworkUtility.instance.SpawnObject(obj);

							//obj.transform.rotation = GetRotationFromNormalSurface(obj);

							int teamColorValue = mapTextureColor.b;
							if (teamColorValue < teams.Length)//everything bigger is an object without a team like forests or rocks
							{
								//set building Team equal to team at index [blue Channel value (0,4)]
								Team team = teams[teamColorValue];


								//TODO SAME CODE AS IN MAINBUILDING
								Building building = obj.GetComponent<Building>();
								building.team = team.teamID;
								obj.GetComponent<Building>().SetLevelMesh();
							}

						}
					}
				}
			}
		}
	}

	private void UpdateMesh()
	{
		mesh.vertices = vertices;
		mesh.uv = uv;
		mesh.triangles = triangles;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
	}

	private void BuildWaterMesh()
	{
		GameObject waterObj = new GameObject("Water", typeof(MeshFilter), typeof(MeshRenderer));
		waterObj.transform.position = new Vector3(xSize / 2, 0, zSize / 2);
		waterObj.transform.parent = transform;
		waterMesh = new Mesh();
		waterObj.GetComponent<MeshFilter>().sharedMesh = waterMesh;
		waterObj.GetComponent<MeshRenderer>().sharedMaterial = waterMaterial;


		waterMesh.name = "Water";
		//initialize vertices and uv Arrays with Texture dimensions
		//Vector3[] waterVerts = new Vector3[(xSize + 1) * (zSize + 1) * (tweenerVerts +1)* 2];
		Vector3[] waterVerts = new Vector3[(xSize + 1) * (zSize + 1)];
		Vector2[] waterUvs = new Vector2[waterVerts.Length];

		for (int i = 0, z = -zSize / 2; z <= zSize / 2; z++)
		{
			for (int x = -zSize / 2; x <= xSize / 2; x++)
			{
				//populate vertex array with default height vertices
				waterVerts[i] = new Vector3(x, 0, z);
				//set uv coordinates
				waterUvs[i] = new Vector2(waterVerts[i].x / xSize, waterVerts[i].z / zSize);
				i++;
			}

		}

		//initialize triangles Array with Texture dimensions
		int[] waterTris = new int[xSize * zSize * 6];
		int tris = 0;

		int limit = waterVerts.Length - (xSize + 1) * (zSize + 1);

		for (int vert = 0; vert < waterTris.Length / 6; vert++)
		{

			waterTris[tris + 0] = vert + 0;
			waterTris[tris + 1] = vert + xSize + 1;
			waterTris[tris + 2] = vert + 1;
			waterTris[tris + 3] = vert + 1;
			waterTris[tris + 4] = vert + xSize + 1;
			waterTris[tris + 5] = vert + xSize + 2;
			tris += 6;
		}

		waterMesh.vertices = waterVerts;
		waterMesh.uv = waterUvs;
		waterMesh.triangles = waterTris;
		waterMesh.RecalculateNormals();
		waterMesh.RecalculateBounds();
	}

	public static Quaternion GetRotationFromNormalSurface(GameObject obj)
	{
		RaycastHit hit;
		// Bit shift the index of the layer(9) (Groundlayer)to get a bit mask
		int layerMask = 1 << 9;

		// Does the ray intersect any objects excluding the player layer
		if (Physics.Raycast(obj.transform.position, Vector3.down, out hit, 10, layerMask))
		{
			return Quaternion.FromToRotation(Vector3.up, hit.normal);
		}
		else
		{
			return Quaternion.identity;
		}
	}
}