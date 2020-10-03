using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;

public class Building : MonoBehaviour, ISelectable
{

    public ResourceManager resourceManager;
    public int team = -1;
    public RenderTexture renderTexture;

    [SerializeField] Mesh[] meshlevels = null;

    public int buildCost = 50;
    public int levelCost { get; private set; } = 100;

    public int level { get; private set; } = 1;
    public bool canLevelUp { get; private set; } = false;

    int maxLevel;

    private void Awake()
    {
        SetLevelMesh();
        
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
        VFXManager.instance.PlayEffect(VFXManager.instance.levelUpEffect, transform.position);
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
            PlacementController.instance.SetCanBuild(Vector3.Distance(ResourceUiManager.instance.activeResourceMan.mainbuilding.transform.position, transform.position) <= PlacementController.instance.maxPlacementRange);
            return;
        }
        if (other.CompareTag("Ground")) return;

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

    public int GetTeam()
    {
        return team;
    }
}
