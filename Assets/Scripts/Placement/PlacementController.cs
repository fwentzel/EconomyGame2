using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Linq;
public class PlacementController : MonoBehaviour
{
    [HideInInspector] public static PlacementController instance { get; private set; }
    public bool isPlacing { get; private set; } = false;
    public int maxPlacementRange { get; private set; } = 4;
    public bool[,] harbourPlacements;
    Material gridMaterial;
    bool canBuild = true;
    GameObject placeableObject;
    Vector3 mousePos;

    int gridSpacing;
    Mesh groundMesh;
    int xSize;
    int zSize;
    Mouse mouse;

    private void Awake()
    {
        //Singleton Check
        if (instance == null)
            instance = this;
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        mouse = Mouse.current;
       
    }

    public void Start()
    {
        MapGenerator mapGen = FindObjectOfType<MapGenerator>();
        xSize = mapGen.xSize;
        zSize = mapGen.zSize;
        groundMesh = mapGen.GetComponent<MeshFilter>().mesh;
        gridMaterial = mapGen.GetComponent<MeshRenderer>().material;
        gridSpacing = mapGen.gridSpacing;
        //gridPlacementOffset = (float)gridSpacing / 2;
        harbourPlacements = new bool[mapGen.xSize, mapGen.zSize];
        SetupHarbourPlacements();
         UpdateGridColor(Color.red);
    }


    private void Update()
    {
        if (placeableObject != null)
        {
            Vector3 currMousePos = Utils.GetMouseGroundPosition(mouse.position.ReadValue());
            if (currMousePos != Vector3.zero)//Utilfunction returns Vector3.zero when mouse is not over Ground
                mousePos = currMousePos;
            MoveCurrentObjectToMouse();
            UpdateGridPosition();
            CheckInput();
        }
    }


    private void SetupHarbourPlacements()
    {
        for (int x = 0; x < harbourPlacements.GetLength(0); x++)
        {
            for (int z = 0; z < harbourPlacements.GetLength(1); z++)
            {
                Vector2 pos = new Vector2(x, z);
                harbourPlacements[x, z] = CheckSurroundingTiles(pos, 0, h => h < 0);
            }
        }
    }

    public bool CheckSurroundingTiles(Vector2 pos, int objDesiredHeight, Predicate<float> heightComparison)
    {
        bool result = false;
        if (GetMeanHeightSurrounding(pos) == objDesiredHeight)
        {
            result = heightComparison(GetMeanHeightSurrounding(pos + Vector2.up)) ||
            heightComparison(GetMeanHeightSurrounding(pos + Vector2.down)) ||
            heightComparison(GetMeanHeightSurrounding(pos + Vector2.left)) ||
            heightComparison(GetMeanHeightSurrounding(pos + Vector2.right));
        }
        return result;

    }

    private void MoveCurrentObjectToMouse()
    {

        int newX = Mathf.RoundToInt((mousePos.x) / gridSpacing);
        int newZ = Mathf.RoundToInt((mousePos.z) / gridSpacing);

        float newY = GetMeanHeightSurrounding(new Vector2(mousePos.x, mousePos.z));
        //readd gridSpacing and offset for final Objectposition
        placeableObject.transform.position = new Vector3((float)newX * gridSpacing, newY,
                                                            (float)newZ * gridSpacing);
    }

    public float GetMeanHeightSurrounding(Vector2 pos)
    {
        //calulate Vertex coordinates by  normalizing  by gridspacing
        int newX = Mathf.RoundToInt(pos.x / gridSpacing);
        int newZ = Mathf.RoundToInt(pos.y / gridSpacing);

        if (newX < 1 || newZ < 1)
            return 0;

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
        if (mouse.leftButton.wasReleasedThisFrame)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                FinishBuildProcess();
            }
        }
        if (mouse.rightButton.wasReleasedThisFrame)
        {
            CancelBuildProcess();
        }
    }

    private void FinishBuildProcess()
    {
        if (!canBuild)
            return;

        //remove unnecessary components and set Trigger false so other Placings detect it with OnTriggerEnter()
        Destroy(placeableObject.GetComponent<Buildcheck>());
        placeableObject.GetComponent<BoxCollider>().isTrigger = false;

        //Set Team variable
        Building building = placeableObject.GetComponent<Building>();
        //TODO PLAYER 0
        GameManager.instance.localPlayer.mainbuilding.AddBuilding(building);

        //toggle off placement grid and reset placeableObject
        placeableObject = null;
        isPlacing = false;
        toggleGrid(0);


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
        //Instantiate new object to place and add necessary components. Make sure Collider is Trigger for OnTriggerEnter()
        //TODO take from pool?
        if (placeableObject != null)
        {
            Destroy(placeableObject);
        }

        placeableObject = Instantiate(placeable);
        placeableObject.GetComponent<Building>().enabled = false;
        placeableObject.GetComponent<Building>().team=GameManager.instance.localPlayer.mainbuilding.team;
        placeableObject.AddComponent<Buildcheck>();
        placeableObject.GetComponent<BoxCollider>().isTrigger = true;
        isPlacing = true;
        UiManager.instance.CloseAll();

        
        ConfigureGrid();
    }


    private void ConfigureGrid()
    {
            Vector3 pos = ResourceUiManager.instance.activeResourceMan.mainbuilding.transform.position;
            Vector4 posRange = new Vector4(pos.x, pos.y, pos.z, maxPlacementRange + .5f);
            gridMaterial.SetVector("MainBuildPos", posRange);
            gridMaterial.SetInt("UseBuildingRange",placeableObject.GetComponent<Building>().UseMaxPlacementRange?1:0);
            SetCanBuild(false);
            toggleGrid(1);        
    }

    private void toggleGrid(int activateGrid)
    {
        //Set float (0,1) in shader to activate or deactivate the grid
        gridMaterial.SetInt("Vector1_81E9F338", activateGrid);
    }

    private void UpdateGridPosition()
    {
        //send current calculated mouseposition to shader
        gridMaterial.SetVector("MousePos", mousePos);
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

