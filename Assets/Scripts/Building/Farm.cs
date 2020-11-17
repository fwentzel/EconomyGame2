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
        int maxPlaceRange = PlacementController.instance.maxPlacementRange;
        for (int x = mainBuildingPos.x - maxPlaceRange; x <= mainBuildingPos.x + maxPlaceRange; x++)
        {
            for (int z = mainBuildingPos.z - maxPlaceRange; z <= mainBuildingPos.z + maxPlaceRange; z++)
            {
                Vector2 pos = new Vector2(x, z);
                float dist = Vector3.Distance(mainBuildingPos, new Vector3(x, 0, z));
                if (PlacementController.instance.CheckSurroundingTiles(pos, 0, h => h == 0) && dist <= maxPlaceRange
                )
                {
                    if (dist == 0)
                        continue;
                    possiblePlacementsCache.Add(new Vector2(x, z));
                }
            }
        }
        base.SetupPossiblePlacements(t);
    }

}
