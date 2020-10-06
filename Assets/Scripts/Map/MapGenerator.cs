#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(MeshFilter))]
public class MapGenerator : MonoBehaviour
{
    public int perlinScale = 30;
    public float heightOffsetStrength;
    public int perlinOffsetRandomness;

    [SerializeField] Texture2D mapTexture = null;
    [SerializeField] Material material = null;

    [SerializeField] Team[] teams = null;
    public ColorToObjectMapping colorToObjectMapping;
    public ColorToHeightMapping colorToHeightMapping;

    Mesh waterMesh;
    public int gridSpacing { get; private set; } = 1;
    public int xSize { get; private set; }
    public int zSize { get; private set; }
    public int waterVertexHeight { get; private set; }
    float waterColorValue;
    public Material waterMaterial = null;

    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;
    Vector2[] uv;


    // TODO Fix / pput in other script
    private void OnMouseUp()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        SelectionManager.instance.Deselect();

    }
    private void Awake()
    {
        SetDimension();

#if !UNITY_EDITOR
        SetupMap();
#endif
    }

    public void SetupMap()
    {

        waterMaterial.SetFloat("StartTime", -999);
        GenerateMap();

    }

    void SetDimension()
    {
        xSize = mapTexture.width - mapTexture.width % 2;
        zSize = mapTexture.height - mapTexture.height % 2;

        for (int i = 0; i < colorToHeightMapping.colorHeightMappings.Length; i++)
        {
            if (colorToHeightMapping.colorHeightMappings[i].name.Equals("Water"))
            {
                waterVertexHeight = colorToHeightMapping.colorHeightMappings[i].vertexHight;
                waterColorValue = colorToHeightMapping.colorHeightMappings[i].value / 255f;
            }
        }
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
        BuildObjectsOnMap();
        material.SetInt("Vector1_2D88299F", gridSpacing);
        material.SetTexture("Texture2D_AD5527E4", mapTexture);
        // NavMeshBuilder.BuildNavMeshData();
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

                foreach (ColorToHeight heightMapping in colorToHeightMapping.colorHeightMappings)
                {
                    //get Texturecolors at vertexposition as well as left, lower and lower left of position
                    Color32 mapTextureColor = mapTexture.GetPixel(x, z);
                    Color32 mapTextureColorLeftX = mapTexture.GetPixel(x - 1, z);
                    Color32 mapTextureColorUpZ = mapTexture.GetPixel(x, z - 1);
                    Color32 mapTextureColorLeftXUpZ = mapTexture.GetPixel(x - 1, z - 1);

                    //check if left/lower texturepixel have same Value as this mapping
                    if (heightMapping.value == mapTextureColor.b || heightMapping.value == mapTextureColorLeftX.b
                        || heightMapping.value == mapTextureColorUpZ.b || heightMapping.value == mapTextureColorLeftXUpZ.b)
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

                foreach (ColorToObject objectMapping in colorToObjectMapping.colorObjectMappings)
                {

                    //check if texture has corresponding green-value as this mapping
                    if (objectMapping.value == mapTextureColor.g)
                    {
                        if (objectMapping.placeable != null)
                        {
                            //ALSO USED IN PLACEMENTCONTROLLER
                            //float newY = vertices[i].y +
                            //			 vertices[i + 1].y +
                            //			 vertices[xSize + i + 1].y +
                            //			 vertices[xSize + i].y;
                            //newY /= 4;
                            float newY = vertices[i].y;
                            newY = 0;
                            //instantiate given prefab and set Position at coordinsates + gridspacing/2 offset and vertexheigth

# if(UNITY_EDITOR)
                            GameObject obj = PrefabUtility.InstantiatePrefab(objectMapping.placeable, transform) as GameObject;
#else
                            GameObject obj = Instantiate(objectMapping.placeable, transform) as GameObject;
#endif
                            obj.transform.position = new Vector3(x + gridSpacing / 2.0f, newY, z + gridSpacing / 2.0f);

                            //obj.transform.rotation = GetRotationFromNormalSurface(obj);

                            int teamColorValue = mapTextureColor.r;

                            if (teamColorValue > 0 && teamColorValue <= teams.Length)//everything bigger 
                            {
                                //set building Team equal to team at index [blue Channel value (1,4)]
                                Team team = teams[teamColorValue - 1];
                                ResourceObject resourceObject = obj.GetComponent<ResourceObject>();
                                if (resourceObject != null)
                                {
                                    resourceObject.team = team.teamID;
                                    resourceObject.OnBuild();
                                    continue;
                                }

                                //TODO SAME CODE AS IN MAINBUILDING
                                Building building = obj.GetComponent<Building>();
                                building.team = team;
                                building.SetLevelMesh();

                                if (building is Harbour)
                                {
                                    obj.transform.rotation = Quaternion.Euler(0, 0, 0);
                                    if (mapTexture.GetPixel(x, z + 1).b == waterColorValue)
                                    {
                                        obj.transform.RotateAround(obj.transform.position, Vector3.up, 90);
                                        obj.transform.position += Vector3.back;
                                        continue;
                                    }
                                    if (mapTexture.GetPixel(x + 1, z).b == waterColorValue)
                                    {
                                        obj.transform.RotateAround(obj.transform.position, Vector3.up, 180);
                                        obj.transform.position += Vector3.left;
                                        continue;
                                    }
                                    if (mapTexture.GetPixel(x, z - 1).b == waterColorValue)
                                    {
                                        obj.transform.RotateAround(obj.transform.position, Vector3.up, -90);
                                        obj.transform.position += Vector3.forward;
                                        continue;

                                    }
                                    obj.transform.position += Vector3.right;
                                }
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
                //populate vertex array with null height vertices
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