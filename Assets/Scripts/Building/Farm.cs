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

    protected override void SetupPossiblePlacements()
    {
        Vector3 buildingPos = transform.position;
        Vector3Int intBuildingPos = new Vector3Int((int)buildingPos.x, (int)buildingPos.y, (int)buildingPos.z);
        int maxPlaceRange = PlacementController.instance.maxPlacementRadius;
        for (int x = intBuildingPos.x - maxPlaceRange; x <= intBuildingPos.x + maxPlaceRange; x++)
        {
            for (int z = intBuildingPos.z - maxPlaceRange; z <= intBuildingPos.z + maxPlaceRange; z++)
            {
                float dist = Mathf.Abs(x - intBuildingPos.x) + Mathf.Abs(z - intBuildingPos.z);
                 if (dist == 0)
                        continue;
                if (PlacementController.instance.CheckSurroundingTiles(new Vector2(x, z), 0, h => h == 0) && dist <= maxPlaceRange)
                {
                    possiblePlacementsCache.Add(new Vector2(x, z));
                }
            }
        }
        base.SetupPossiblePlacements();
    }

}
