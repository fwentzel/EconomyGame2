using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Linq;

public class PlacementController : MonoBehaviour
{
    [HideInInspector] public static PlacementController instance { get; private set; }
    public bool isPlacing { get; private set; } = false;
    public int maxPlacementRadius { get; private set; } =2;

    public bool canBuild { get; private set; } = true;
    public Building closestBuilding { get; private set; }

    public Rock[] rocks;
    Material gridMaterial;
    GameObject placeableObject;
    Vector3 mousePos;

    int gridSpacing;
    Mesh groundMesh;
    int xSize;
    int zSize;
    Mouse mouse;

    bool useMaxPlacementRange = false;

    List<GameObject> buildSpotMarker = new List<GameObject>();


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
        rocks = FindObjectsOfType<Rock>();

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

    public bool CheckSurroundingTiles(Vector2 pos, int objDesiredHeight, Predicate<float> heightComparison)
    {
        bool result = false;
        if (GetMeanHeightSurrounding(pos) == objDesiredHeight)
        {
            result =
            heightComparison(GetMeanHeightSurrounding(pos + Vector2.up)) ||
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

        foreach (GameObject obj in buildSpotMarker)
        {
            Destroy(obj);
        }
        buildSpotMarker.Clear();


    }

    private void CancelBuildProcess()
    {
        //toggle off placement Grid and Destroy the building 
        //TODO Pooling?
        isPlacing = false;
        toggleGrid(0);
        Destroy(placeableObject);

        foreach (GameObject obj in buildSpotMarker)
        {
            Destroy(obj);
        }
        buildSpotMarker.Clear();
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
        Building building = placeableObject.GetComponent<Building>();
        building.enabled = false;
        building.team = GameManager.instance.localPlayer.mainbuilding.team;
        building.GetPossibleBuildSpots();
        placeableObject.AddComponent<Buildcheck>();
        placeableObject.GetComponent<BoxCollider>().isTrigger = true;
        isPlacing = true;
        UiManager.instance.CloseAll();


        ConfigureGrid();
    }


    private void ConfigureGrid()
    {
        SetGridPosToNearestBuilding();
        Building building = placeableObject.GetComponent<Building>();
        useMaxPlacementRange = building.spotType==buildSpotType.normal;
        if (useMaxPlacementRange)
        {
            gridMaterial.SetInt("UseBuildingRange", 1);
        }
        else
        {
            gridMaterial.SetInt("UseBuildingRange", 0);
            //Harbour or Mine
            ShowPlaceholderForBuildSpots(building);
        }

        SetCanBuild(false);
        toggleGrid(1);
    }

    private void SetGridPosToNearestBuilding()
    {
        Mainbuilding mainbuilding = ResourceUiManager.instance.activeResourceMan.mainbuilding;
        closestBuilding = mainbuilding.buildings.OrderBy(t => (t.transform.position - mousePos).sqrMagnitude).First();
        if ((mainbuilding.transform.position - mousePos).sqrMagnitude < (closestBuilding.transform.position - mousePos).sqrMagnitude)
            closestBuilding = mainbuilding;

        Vector4 posRange = new Vector4(closestBuilding.transform.position.x, closestBuilding.transform.position.y, closestBuilding.transform.position.z, maxPlacementRadius + .5f);
        gridMaterial.SetVector("MainBuildPos", posRange);
    }

    private void ShowPlaceholderForBuildSpots(Building building)
    {
        //TODO POOLING!
        foreach (Vector2 spot in PlacementSpotsManager.spotsForBuildingTypeAndTeam[building.spotType][building.team])
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.GetComponent<BoxCollider>().enabled = false;
            cube.transform.position = new Vector3(spot.x, 0.3f, spot.y);
            cube.transform.localScale = new Vector3(.2f, .2f, .2f);

        }
    }
    private void toggleGrid(int activateGrid)
    {
        //Set float (0,1) in shader to activate or deactivate the grid
        gridMaterial.SetInt("Vector1_81E9F338", activateGrid);
    }

    private void UpdateGridPosition()
    {
        if (useMaxPlacementRange)
            SetGridPosToNearestBuilding();
        //send current calculated mouseposition to shader
        gridMaterial.SetVector("MousePos", mousePos);
    }

    private void UpdateGridColor(Color gridColor)
    {
        //set gridcolor in shader
        gridMaterial.SetColor("Color_31DF09FF", gridColor);
    }

    public void SetCanBuild(buildSpotType spotType, Team team, Vector2 pos)
    {
        //Set canBuildvariable and set Gridcolor depending on value of _canBuild
        this.canBuild = PlacementSpotsManager.spotsForBuildingTypeAndTeam[spotType][team].Contains(pos);
        if (canBuild)
            UpdateGridColor(Color.white);
        else
            UpdateGridColor(Color.red);


    }
    public void SetCanBuild(Boolean canBuild)
    {
        //Set canBuildvariable and set Gridcolor depending on value of _canBuild
        this.canBuild = canBuild;
        if (canBuild)
            UpdateGridColor(Color.white);
        else
            UpdateGridColor(Color.red);


    }
}

