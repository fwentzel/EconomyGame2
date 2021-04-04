using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlacementController : MonoBehaviour
{
    [HideInInspector] public static PlacementController instance { get; private set; }
    public bool isPlacing { get; private set; } = false;
    public int maxPlacementRadius { get; private set; } = 4;

    public bool canBuild { get; private set; } = true;

    public Rock[] rocks;
    Material gridMaterial;
    GameObject placeableObject;
    Vector3 mousePos;

    int gridSpacing;
    Mesh groundMesh;
    int xSize;
    int zSize;
    Mouse mouse;

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
        building.GetPossibleBuildSpots(GameManager.instance.localPlayer.mainbuilding.team);
        placeableObject.AddComponent<Buildcheck>();
        placeableObject.GetComponent<BoxCollider>().isTrigger = true;
        isPlacing = true;
        UiManager.instance.CloseAll();


        ConfigureGrid();
    }


    private void ConfigureGrid()
    {
        Vector3 pos = ResourceUiManager.instance.activeResourceMan.mainbuilding.transform.position;
        Vector4 posRange = new Vector4(pos.x, pos.y, pos.z, maxPlacementRadius + .5f);
        gridMaterial.SetVector("MainBuildPos", posRange);
        Building building = placeableObject.GetComponent<Building>();
        if (building.UseMaxPlacementRange)
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

    private void ShowPlaceholderForBuildSpots(Building building)
    {
        //TODO POOLING!
        List<BuildingPlacementInfo> buildingPlacementInfos = PlacementSpotsManager.spots[building.GetType()];

        foreach (BuildingPlacementInfo info in buildingPlacementInfos)
        {
            if (info.team == building.team)
            {
                foreach (Vector2 spot in info.possibleSpots)
                {
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.GetComponent<BoxCollider>().enabled = false;
                    cube.transform.position = new Vector3(spot.x, 0.3f, spot.y);
                    cube.transform.localScale = new Vector3(.2f, .2f, .2f);

                }

            }
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

