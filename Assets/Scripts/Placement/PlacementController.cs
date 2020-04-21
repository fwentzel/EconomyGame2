using UnityEngine;
using UnityEngine.EventSystems;

public class PlacementController : MonoBehaviour
{
	[HideInInspector] public static PlacementController instance { get; private set; }
	public bool isPlacing = false;
	Material gridMaterial;
	bool canBuild = true;
	GameObject placeableObject;
	Vector3 mousePos;
	int gridSpacing;
	float gridPlacementOffset;
	Mesh groundMesh;
	int xSize;
	int zSize;
	private void Awake()
	{
		//Singleton Check
		if (instance == null)
			instance = this;
		else if (instance != this)
		{
			Destroy(gameObject);
		}
	}

	public void SetupGridParameter()
	{
		MapGenerator mapGen = FindObjectOfType<MapGenerator>();
		xSize = mapGen.xSize;
		zSize = mapGen.zSize;
		groundMesh = mapGen.GetComponent<MeshFilter>().mesh;
		gridMaterial = mapGen.GetComponent<MeshRenderer>().material;
		gridSpacing = mapGen.GetComponent<MapGenerator>().gridSpacing;
		gridPlacementOffset = (float)gridSpacing / 2;
	}

	private void Update()
	{
		if (placeableObject != null)
		{
			GetMouseWorldPosition();
			MoveCurrentObjectToMouse();
			UpdateGridPosition();
			CheckInput();
		}
	}

	private void GetMouseWorldPosition()
	{
		//Get Mouseposition in World coordinates on Ground Collider
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitInfo;
		int layermask = LayerMask.GetMask("Ground");
		if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, layermask))
		{
			mousePos = hitInfo.point;
		}
	}

	private void MoveCurrentObjectToMouse()
	{
		//calulate coordinates with normalizing by gridspacing
		int newX = Mathf.RoundToInt((mousePos.x + gridPlacementOffset) / gridSpacing);
		int newZ = Mathf.RoundToInt((mousePos.z + gridPlacementOffset) / gridSpacing);
		float newY = GetMeanHeightSurrounding(newX, newZ);
		//readd gridSpacing and offset for final Objectposition
		placeableObject.transform.position = new Vector3((float)newX * gridSpacing - gridPlacementOffset, newY,
															(float)newZ * gridSpacing - gridPlacementOffset);
	}

	private float GetMeanHeightSurrounding(int newX, int newZ)
	{
		//ALSO USED IN MMAPGENERATION
		int size = xSize + 1;
		float newY = groundMesh.vertices[((newZ - 1) * size) + newX].y +
					groundMesh.vertices[((newZ - 1) * size) + newX - 1].y +
					 groundMesh.vertices[(newZ * size) + newX - 1].y +
					groundMesh.vertices[(newZ * size) + newX].y;
		newY /= 4;
		return newY;
	}
	public float GetMeanHeightSurrounding(Vector2 xz)
	{
		int newX = Mathf.RoundToInt((xz.x + gridPlacementOffset) / gridSpacing);
		int newZ = Mathf.RoundToInt((xz.y + gridPlacementOffset) / gridSpacing);
		//ALSO USED IN MMAPGENERATION
		int size = xSize + 1;
		float newY = groundMesh.vertices[((newZ - 1) * size) + newX].y +
					groundMesh.vertices[((newZ - 1) * size) + newX - 1].y +
					 groundMesh.vertices[(newZ * size) + newX - 1].y +
					groundMesh.vertices[(newZ * size) + newX].y;
		newY /= 4;
		return newY;
	}

	private void CheckInput()
	{
		if (Input.GetMouseButtonUp(0))
		{
			if (!EventSystem.current.IsPointerOverGameObject())
			{
				FinishBuildProcess();
			}
		}
		if (Input.GetMouseButtonUp(1))
		{
			CancelBuildProcess();
		}
	}

	private void FinishBuildProcess()
	{
		if (!canBuild)
		{
			return;
		}
		else
		{
			//remove unnecessary components and set Trigger false so other Placings detect it with OnTriggerEnter()
			Destroy(placeableObject.GetComponent<Buildcheck>());
			placeableObject.GetComponent<BoxCollider>().isTrigger = false;

			//spawn Object on server so all clients can see
			NetworkUtility.instance.SpawnObject(placeableObject);

			//Set Team variable
			Building building = placeableObject.GetComponent<Building>();
			//TODO PLAYER 0
			GameManager.instance.localPlayer.mainBuilding.AddBuilding(building);

			//toggle off placement grid and reset placeableObject
			placeableObject = null;
			isPlacing = false;
			toggleGrid(0);
		}
	}

	private void CancelBuildProcess()
	{
		//toggle off placement Grid and Destroy the building 
		//TODO Pooling?
		isPlacing = false;
		toggleGrid(0);
		Destroy(placeableObject);
	}

	public void NewPlaceableObject(GameObject placeable)
	{
		//Turn on placement Grid and set Color to White
		ConfigureGrid(true);

		//Instantiate new object to place and add necessary components. Make sure Collider is Trigger for OnTriggerEnter()
		//TODO take from pool?
		if (placeableObject != null)
		{
			Destroy(placeableObject);
		}
		placeableObject = Instantiate(placeable);
		placeableObject.GetComponent<Building>().enabled = false;
		placeableObject.AddComponent<Buildcheck>();
		placeableObject.GetComponent<BoxCollider>().isTrigger = true;
		isPlacing = true;
	}

	private void ConfigureGrid(bool isStart)
	{
		if (isStart)
		{
			toggleGrid(1);
			UpdateGridColor(Color.white);
		}
		else
		{
			toggleGrid(0);
		}
	}

	private void toggleGrid(int activateGrid)
	{
		//Set float (0,1) in shader to activate or deactivate the grid
		gridMaterial.SetInt("Vector1_81E9F338", activateGrid);
	}

	private void UpdateGridPosition()
	{
		//send current calculated mouseposition to shader
		gridMaterial.SetVector("Vector3_F58EFE40", mousePos);
	}

	private void UpdateGridColor(Color gridColor)
	{
		//set gridcolor in shader
		gridMaterial.SetColor("Color_31DF09FF", gridColor);
	}

	public void SetCanBuild(bool canBuild)
	{
		//Set canBuildvariable and set Gridcolor depending on value of _canBuild
		this.canBuild = canBuild;
		if (canBuild)
			UpdateGridColor(Color.white);
		else
			UpdateGridColor(Color.red);


	}
}

