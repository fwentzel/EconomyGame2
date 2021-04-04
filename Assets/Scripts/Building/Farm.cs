using System;
using UnityEngine;

public class Farm : Building
{
    public int unitsPerIntervall;


    public override void OnBuild(bool subtractResource = true)
    {
        resourceManager.ChangeRessourceAmount(resource.food, unitsPerIntervall);
        base.OnBuild(subtractResource);
    }

    public override void DestroyBuilding()
    {
        resourceManager.ChangeRessourceAmount(resource.food, -unitsPerIntervall);
        base.DestroyBuilding();
    }

    protected override void OnLevelUp()
    {
        base.OnLevelUp();
        resourceManager.ChangeRessourceAmount(resource.food, unitsPerIntervall);
        unitsPerIntervall *= 2;

    }

    protected override void TriggerBonusLevel()
    {
        unitsPerIntervall += (unitsPerIntervall / 2);
    }

    public override string GetStats()
    {
        return $"Farm\nLevel {level}\nGenerates {unitsPerIntervall} food";
    }

    protected override void SetupPossiblePlacements(Team t)
    {
        Vector3 tempMainPos = Array.Find(CitysMeanResource.instance.resourceManagers, resourceManager => resourceManager.mainbuilding.team == t).transform.position;
        Vector3Int mainBuildingPos = new Vector3Int((int)tempMainPos.x, (int)tempMainPos.y, (int)tempMainPos.z);
        int maxPlaceRange = PlacementController.instance.maxPlacementRadius;
        for (int x = mainBuildingPos.x - maxPlaceRange; x <= mainBuildingPos.x + maxPlaceRange; x++)
        {
            for (int z = mainBuildingPos.z - maxPlaceRange; z <= mainBuildingPos.z + maxPlaceRange; z++)
            {
                float dist = Mathf.Abs(x - mainBuildingPos.x) + Mathf.Abs(z - mainBuildingPos.z);
                 if (dist == 0)
                        continue;
                if (PlacementController.instance.CheckSurroundingTiles(new Vector2(x, z), 0, h => h == 0) && dist <= maxPlaceRange)
                {
                    possiblePlacementsCache.Add(new Vector2(x, z));
                }
            }
        }
        base.SetupPossiblePlacements(t);
    }

}
