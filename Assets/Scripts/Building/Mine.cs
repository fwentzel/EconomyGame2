using UnityEngine;
using System;
public class Mine : Building
{
    public int unitsPerIntervall;

    private Collider[] overlapResults = new Collider[5];

    private void Awake()
    {
        UseMaxPlacementRange = false;
    }

    public override void OnBuild(bool subtractResource = true)
    {

        ResourceObject rock = Array.Find<ResourceObject>(PlacementController.instance.resourceObjects, rocks => rocks.transform.position == transform.position);
        if (rock != null) rock.occupied = true;

        resourceManager.ChangeRessourceAmount(resource.stone, unitsPerIntervall);
        base.OnBuild(subtractResource);
    }

    public override void DestroyBuilding()
    {
        resourceManager.ChangeRessourceAmount(resource.stone, -unitsPerIntervall);
        base.DestroyBuilding();
    }

    // protected override void SetupPossiblePlacements()
    // {
    //     ResourceObject[] rocks = Array.FindAll<ResourceObject>(PlacementController.instance.resourceObjects, rock => rock.CompareTag("RockResource") && rock.team == team);
    //     foreach (ResourceObject rock in rocks)
    //     {
    //         possiblePlacements[team].Add(new Vector2(rock.transform.position.x, rock.transform.position.z));
    //     }
    // }
    protected override void OnLevelUp()
    {
        base.OnLevelUp();
        resourceManager.ChangeRessourceAmount(resource.stone, unitsPerIntervall);
        unitsPerIntervall *= 2;
    }
    protected override void TriggerBonusLevel()
    {
        unitsPerIntervall *= 2;
    }
    public override string GetStats()
    {
        return $"Mine\nLevel {level}\nGenerates {unitsPerIntervall} Stone";
    }

    public override void CheckCanBuild(Collider other, bool onEnter)
    {
        if (other == null) return;

        if (onEnter&&possibleDefaultPlacements[team].Contains(new Vector2(transform.position.x, transform.position.z)))
        {       
            ResourceObject resourceObject=other.GetComponent<ResourceObject>();
            PlacementController.instance.SetCanBuild(resourceObject!=null&&!resourceObject.occupied);
        }
        else{
            PlacementController.instance.SetCanBuild(false);
        }


    }
}
