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
    public buildSpotType spotType { get; protected set; } = buildSpotType.normal;
    public HashSet<Vector2> possiblePlacementsCache = new HashSet<Vector2>();
    int maxLevel;

    private void Awake()
    {
        SetLevelMesh();
    }

    private void OnDrawGizmos()
    {
         if (!PlacementSpotsManager.spotsForBuildingTypeAndTeam.ContainsKey(spotType)||!PlacementSpotsManager.spotsForBuildingTypeAndTeam[spotType].ContainsKey(team))
        {
            return;
        }
       
        Gizmos.color=Color.blue;
       foreach (var item in PlacementSpotsManager.spotsForBuildingTypeAndTeam[spotType][team])
       {
            Gizmos.DrawCube(new Vector3(item.x, 0, item.y), new Vector3(.2f, .2f, .2f));
       }
    }
    protected virtual void SetupPossiblePlacements()
    {
        HashSet<Vector2> set = PlacementSpotsManager.spotsForBuildingTypeAndTeam[spotType][team];
        foreach (Vector2 item in possiblePlacementsCache)
        {
            set.Add(item);
        }
        PlacementSpotsManager.spotsForBuildingTypeAndTeam[spotType][team] = set;
        possiblePlacementsCache = new HashSet<Vector2>();
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
        VFXManager.instance.PlayEffect(transform.position, effect.LEVEL_UP);
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

    public virtual void GetPossibleBuildSpots()
    {
        if (!PlacementSpotsManager.spotsForBuildingTypeAndTeam.ContainsKey(spotType))
        {
            //Building not registered yet
            PlacementSpotsManager.spotsForBuildingTypeAndTeam[spotType] = new Dictionary<Team, HashSet<Vector2>>();
        }

        if (!PlacementSpotsManager.spotsForBuildingTypeAndTeam[spotType].ContainsKey(team))
        {
            //Team not registered yet
            PlacementSpotsManager.spotsForBuildingTypeAndTeam[spotType][team] = new HashSet<Vector2>();
        }
        SetupPossiblePlacements();

    }


    public virtual void OnBuild(bool subtractResource = true)
    {
        if (subtractResource)
        {
            VFXManager.instance.PlayEffect(transform.position, effect.BUILD);
            resourceManager.ChangeRessourceAmount(resource.gold, -buildCost);
        }
        levelCost = Mathf.RoundToInt(buildCost * 2.5f);
        maxLevel = meshlevels.Length + 1;
        GetPossibleBuildSpots();
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
            PlacementController.instance.SetCanBuild(spotType, team, new Vector2(transform.position.x, transform.position.z));
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
