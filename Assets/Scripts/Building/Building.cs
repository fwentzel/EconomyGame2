using UnityEngine;
using System;
using System.Collections.Generic;

public class Building : MonoBehaviour, ISelectable
{
    protected static Dictionary<Team, List<Vector2>> possibleDefaultPlacements;

    public ResourceManager resourceManager;
    public Team team = null;
    public RenderTexture renderTexture;

    [SerializeField] Mesh[] meshlevels = null;

    public int buildCost = 50;
    public int levelCost { get; private set; } = 100;

    public int level { get; private set; } = 1;
    public bool canLevelUp { get; private set; } = false;
    public bool UseMaxPlacementRange { get; protected set; } = true;

    int maxLevel;

    private void Awake()
    {
        SetLevelMesh();

    }
    private void Start()
    {
        //not initialized yet
        if (possibleDefaultPlacements == null)
            possibleDefaultPlacements = new Dictionary<Team, List<Vector2>>();

        if (!possibleDefaultPlacements.ContainsKey(team))
        {
            possibleDefaultPlacements[team]=new List<Vector2>();
            if (GameManager.instance.dayIndex == 0)//Game not started
            {
                GameManager.instance.OnGameStart += SetupPossiblePlacements;
            }
            else
            {
                //Game running, callback wont be called
                SetupPossiblePlacements();
            }

        }
    }

    protected virtual void SetupPossiblePlacements()
    {
        MapGenerator mapGenerator = FindObjectOfType<MapGenerator>();
        for (int x = 0; x < mapGenerator.xSize; x++)
        {
            for (int z = 0; z < mapGenerator.zSize; z++)
            {
                Vector2 pos = new Vector2(x, z);
                if (PlacementController.instance.CheckSurroundingTiles(pos, 0, h => h == 0) &&
                Vector3.Distance(ResourceUiManager.instance.activeResourceMan.mainbuilding.transform.position, new Vector3(x,0,z)) <= PlacementController.instance.maxPlacementRange)
                {
                    possibleDefaultPlacements[team].Add(new Vector2(x, z));
                }
            }
        }
    }



    public String GetLevelCostString()
    {
        return level == maxLevel ? "MAX" : levelCost.ToString();
    }
    public bool LevelUp()
    {
        if (level == maxLevel)
            return false;
        level++;
        OnLevelUp();
        return true;
    }

    protected virtual void OnLevelUp()
    {
        if (level == maxLevel)
            TriggerBonusLevel();

        resourceManager.ChangeRessourceAmount(resource.gold, -levelCost);
        VFXManager.PlayEffect(transform.position);
        SetLevelMesh();
        levelCost = (int)(levelCost * 1.5f);
        CheckCanLevelUp();

    }

    protected virtual void TriggerBonusLevel() { }

    public void SetLevelMesh()
    {
        if (meshlevels.Length > level - 1)
        {
            GetComponent<MeshFilter>().sharedMesh = meshlevels[level - 1];
            GetComponent<BoxCollider>().size = meshlevels[level - 1].bounds.extents * 2;
            GetComponent<BoxCollider>().center = meshlevels[level - 1].bounds.center;
        }
    }

    public static List<Vector2> GetPossibleBuildSpots(Team t){
        return possibleDefaultPlacements[t];
    }
    public virtual void OnBuild(bool subtractResource = true)
    {
        if (subtractResource)
            resourceManager.ChangeRessourceAmount(resource.gold, -buildCost);
        levelCost = Mathf.RoundToInt(buildCost * 2.5f);
        maxLevel = meshlevels.Length;

    }

    public virtual void DestroyBuilding()
    {
        resourceManager.ChangeRessourceAmount(resource.gold, (int)(buildCost * .6f));
        resourceManager.mainbuilding.buildings.Remove(this);
        Destroy(this.gameObject);
    }


    public virtual string GetStats()
    {
        return "Type: Building" + "\nTeam: " + team + "\nLevel: " + level;
    }

    public virtual void CheckCanBuild(Collider other, bool onEnter)
    {
        //only distance check
        if (other == null)
        {
            PlacementController.instance.SetCanBuild(possibleDefaultPlacements[team].Contains(new Vector2(transform.position.x, transform.position.z)));
            return;
        }

        //entered a collieder, so disable build
        if (onEnter)
        {
            PlacementController.instance.SetCanBuild(false);
        }
        else
        {
            //TODO
            PlacementController.instance.SetCanBuild(true);
        }
    }

    public bool CheckCanLevelUp()
    {
        return resourceManager.GetAmount(resource.gold) >= levelCost && level < maxLevel;
    }

    public virtual bool IsSelectable()
    {
        return team == GameManager.instance.localPlayer.team;
    }
}
