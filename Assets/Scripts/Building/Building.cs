using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class Building : MonoBehaviour, ISelectable
{
    public ResourceManager resourceManager;
    public Team team = null;
    public RenderTexture renderTexture;

    [SerializeField] Mesh[] meshlevels = null;

    public int buildCost = 50;
    public int levelCost { get; private set; } = 100;

    public int level { get; private set; } = 1;
    public bool canLevelUp { get; private set; } = false;
    public bool UseMaxPlacementRange { get; protected set; } = true;
    public List<Vector2> possiblePlacementsCache = new List<Vector2>();
    int maxLevel;

    private void Awake()
    {
        SetLevelMesh();
    }

    // private void OnDrawGizmos()
    // {
    //     Dictionary<Type,Color> colormapping= new Dictionary<Type, Color>{{typeof(Harbour),Color.blue}, {typeof(House),Color.red},{typeof(Farm),Color.yellow},{typeof(Mine),Color.black}};
    //     if (PlacementSpotsManager.spots.ContainsKey(GetType()))
    //     {
    //         BuildingPlacementInfo info = Utils.GetBuildInfoForTeam(GetType(),team);
    //         if (info != null && info.possibleSpots.Count > 0)
    //         {
    //             foreach (var item in info.possibleSpots)
    //             {
    //                 Gizmos.color=colormapping[this.GetType()];
    //                 Gizmos.DrawCube(new Vector3(item.x, 0, item.y), new Vector3(1, 1, 1));
    //             }
    //         }
    //     }
    // }
    protected virtual void SetupPossiblePlacements(Team t)
    {
        Transform mainBuilding = Array.Find<ResourceManager>(CitysMeanResource.instance.resourceManagers, r => r.mainbuilding.team == t).transform;
        Vector2 mainPos = new Vector2(mainBuilding.position.x, mainBuilding.position.z);
        possiblePlacementsCache = possiblePlacementsCache.OrderBy(spot => Vector2.Distance(mainPos, spot)).ToList();

        PlacementSpotsManager.spots[GetType()].Add(new BuildingPlacementInfo(t, possiblePlacementsCache));
        possiblePlacementsCache = new List<Vector2>();
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

    public virtual void GetPossibleBuildSpots(Team t)
    {
        if (!PlacementSpotsManager.spots.ContainsKey(GetType()))
        {
            //Building not registered yet
            PlacementSpotsManager.spots[GetType()] = new List<BuildingPlacementInfo>();
        }
        BuildingPlacementInfo info = Utils.GetBuildInfoForTeam(GetType(),t);
        if (info == null || info.possibleSpots.Count == 0)
        {
            SetupPossiblePlacements(t);
        }
    }


    public virtual void OnBuild(bool subtractResource = true)
    {
        if (subtractResource)
            resourceManager.ChangeRessourceAmount(resource.gold, -buildCost);
        levelCost = Mathf.RoundToInt(buildCost * 2.5f);
        maxLevel = meshlevels.Length + 1;

    }

    public virtual void DestroyBuilding()
    {
        resourceManager.ChangeRessourceAmount(resource.gold, (int)(buildCost * .6f));
        resourceManager.mainbuilding.buildings.Remove(this);
        Destroy(this.gameObject);
    }


    public virtual string GetStats()
    {
        return "Type: Building" + "\nTeam: " + team + "\nLevel: " + (level == maxLevel ? level.ToString() : "MAX");
    }

    public virtual void CheckCanBuild(Collider other, bool onEnter)
    {
        //only distance check
        if (other == null)
        {
            PlacementController.instance.SetCanBuild(Utils.GetBuildInfoForTeam(GetType(),team).possibleSpots.Contains(new Vector2(transform.position.x, transform.position.z)));
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
