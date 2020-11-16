using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class Building : MonoBehaviour, ISelectable
{
    protected List<Vector2> possibleDefaultPlacements;
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

    protected virtual void SetupPossiblePlacements(Team t)
    {
        Vector3 tempMainPos = Array.Find(CitysMeanResource.instance.resourceManagers, resourceManager => resourceManager.mainbuilding.team == t).transform.position;
        Vector3Int mainBuildingPos = new Vector3Int((int)tempMainPos.x, (int)tempMainPos.y, (int)tempMainPos.z);
        int maxPlaceRange = PlacementController.instance.maxPlacementRange;
        for (int x = mainBuildingPos.x - maxPlaceRange; x <= mainBuildingPos.x + maxPlaceRange; x++)
        {
            for (int z = mainBuildingPos.z - maxPlaceRange; z <= mainBuildingPos.x + maxPlaceRange; z++)
            {
                Vector2 pos = new Vector2(x, z);
                float dist = Vector3.Distance(mainBuildingPos, new Vector3(x, 0, z));
                if (PlacementController.instance.CheckSurroundingTiles(pos, 0, h => h == 0) && dist <= maxPlaceRange
                )
                {
                    if (dist == 0)
                        continue;
                    possibleDefaultPlacements.Add(new Vector2(x, z));
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

    public virtual List<Vector2> GetPossibleBuildSpots(Team t)
    {
        //not initialized yet
        if (possibleDefaultPlacements == null)
            possibleDefaultPlacements = new List<Vector2>();

        SetupPossiblePlacements(t);
        Transform mainBuilding = Array.Find<ResourceManager>(CitysMeanResource.instance.resourceManagers, r => r.mainbuilding.team == t).transform;
        Vector2 mainPos = new Vector2(mainBuilding.position.x, mainBuilding.position.z);
        possibleDefaultPlacements = possibleDefaultPlacements.OrderBy(spot => Vector2.Distance(mainPos, spot)).ToList();

        return possibleDefaultPlacements;
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
            PlacementController.instance.SetCanBuild(possibleDefaultPlacements.Contains(new Vector2(transform.position.x, transform.position.z)));
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
