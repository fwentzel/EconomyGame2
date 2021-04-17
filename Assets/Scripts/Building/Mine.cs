using UnityEngine;
using System;
using System.Collections.Generic;

public class Mine : Building
{
    public int unitsPerIntervall;

    private Collider[] overlapResults = new Collider[5];

    public override void OnBuild(bool subtractResource = true)
    {
        Rock rock = Array.Find<Rock>(PlacementController.instance.rocks, rocks => rocks.transform.position == transform.position);
        if (rock != null) rock.occupied = true;

        resourceManager.ChangeRessourceAmount(resource.stone, unitsPerIntervall);
        base.OnBuild(subtractResource);
    }

    public override void DestroyBuilding()
    {
        resourceManager.ChangeRessourceAmount(resource.stone, -unitsPerIntervall);
        base.DestroyBuilding();
    }
    protected override void SetupPossiblePlacements()
    {
        Rock[] rocks = Array.FindAll<Rock>(PlacementController.instance.rocks, rock => rock.team == team);
        foreach (Rock rock in rocks)
        {
            possiblePlacementsCache.Add(new Vector2(rock.transform.position.x, rock.transform.position.z));
        }
        base.SetupPossiblePlacements();


    }
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

        if (onEnter &&PlacementSpotsManager.spotsForBuildingTypeAndTeam[spotType][team].Contains(new Vector2(transform.position.x, transform.position.z)))
        {
            Rock rock = other.GetComponent<Rock>();
            PlacementController.instance.SetCanBuild(rock != null && !rock.occupied);
        }
        else
        {
            PlacementController.instance.SetCanBuild(false);
        }


    }
}
