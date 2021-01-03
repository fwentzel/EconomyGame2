using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingAi : BaseUtilityAi
{
    Dictionary<Type, List<Building>> buildingList;
    int previousEnd;
    int gold;
    bool builtHarbour = false;

    public BuildingAi(AiMaster master) : base(master)
    {
        GetSpecificsFromBuilding();

    }

    public bool UpgradeOrBuild(Type type)
    {
        gold = resourceManager.GetAmount(resource.gold);

        //Try Levelling Up
        foreach (var building in buildingList[type])
        {
            if (gold >= building.levelCost && building.LevelUp())
                return true;
        }

        //check if there is enough money to build new Building
        if (buildingList[type].Count > 0 && gold < buildingList[type][0].buildCost)
            return false;

        //Didnt Level up, so we need a new one
        return PlaceBuilding(type);
    }



    //duplicatin code
    private void GetSpecificsFromBuilding()
    {
        buildingList = new Dictionary<Type, List<Building>>() {
            {typeof(House),new List<Building>() },
            {typeof(Farm),new List<Building>() },
            {typeof(Mine),new List<Building>() },
            {typeof(Harbour),new List<Building>() }
        };

        foreach (Building building in mainbuilding.buildings)
        {
            buildingList[building.GetType()].Add(building);
        }
    }



    private bool PlaceBuilding(Type type)
    {
        Building addedBuilding = mainbuilding.AddBuilding(type);
        if (addedBuilding != null)
        {
            buildingList[type].Add(addedBuilding);
            return true;
        }
        return false;
    }

}