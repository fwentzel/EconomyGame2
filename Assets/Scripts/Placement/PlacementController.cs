using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlacementController : MonoBehaviour
{
    [HideInInspector] public static PlacementController instance { get; private set; }
    public bool isPlacing { get; private set; } = false;
    public int maxPlacementRange { get; private set; } = 4;
    Material gridMaterial;
    bool canBuild = true;
    GameObject placeableObject;
    Vector3 mousePos;

    int gridSpacing;
    float gridPlacementOffset;
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
        gridSpacing = mapGen.GetComponent<MapGenerator>().gridSpacing;
        gridPlacementOffset = (float)gridSpacing / 2;
    }

    private void Update()
    {
        if (placeableObject != null)
        {
            Vector3 currMousePos = Utils.GetMouseGroundPosition(mouse.position.ReadValue());
            if (currMousePos != Vector3.zero)//Utilfunction returns Vector3.zero when mouse is not over Ground
                mousePos = currMousePos;
            MoveCurrentObjectToMouse();
            //UpdateGridPosition();
            CheckInput();
        }
    }



    private void MoveCurrentObjectToMouse()
    {
        //calulate coordinates with normalizing by gridspacing
        int newX = Mathf.RoundToInt((mousePos.x + gridPlacementOffset) / gridSpacing);
        int newZ = Mathf.RoundToInt((mousePos.z + gridPlacementOffset) / gridSpacing);
        if (newX == 0 || newZ == 0)
            return;
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
        placeableObject.AddComponent<Buildcheck>();
        placeableObject.GetComponent<BoxCollider>().isTrigger = true;
        isPlacing = true;
        UiManager.instance.CloseAll();

        //Turn on placement Grid and set Color to White
        ConfigureGrid(true);
    }

    private void ConfigureGrid(bool isStart)
    {
        if (isStart)
        {
            Vector3 pos = ResourceUiManager.instance.activeResourceMan.mainbuilding.transform.position;
            Vector4 posRange = new Vector4(pos.x, pos.y, pos.z, maxPlacementRange + .5f);
            if (placeableObject.GetComponent<Building>().IgnoreMaxPlacementRange)
                posRange.w *= 3;
            gridMaterial.SetVector("MainBuildPos", posRange);

            toggleGrid(1);


        }
        else
        {
            UpdateGridColor(Color.red);
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

