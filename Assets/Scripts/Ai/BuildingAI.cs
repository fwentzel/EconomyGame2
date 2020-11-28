using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingAi : BaseAi
{
    Dictionary<Type, List<Building>> buildingList;
    int previousEnd;
    int gold;
    bool builtHarbour = false;

    public BuildingAi(AiMaster master) : base(master)
    {
        GetSpecificsFromBuilding();

    }

    public override Type Tick()
    {
        gold = resourceManager.GetAmount(resource.gold);

        int foodChange = resourceManager.CalculateFoodGenerated() - resourceManager.GetAmount(resource.citizens) * mainbuilding.foodPerDayPerCitizen;

        if (resourceManager.foodChange <= -5 || resourceManager.isLoyaltyDecreasing)
        {
            if (UpgradeOrBuild(typeof(Farm)))
                return typeof(TradeAi);
        }

        if (CitizenManager.instance.freeCitizensPerTeam[resourceManager.mainbuilding.team.teamID].Count == 0
        && (resourceManager.foodChange >= 0
        || resourceManager.GetAmount(resource.food) > (resourceManager.GetAmount(resource.citizens) * mainbuilding.foodPerDayPerCitizen) * 2))//double the food that is needed for ctizens
        {
            //Act and Build 
            if (UpgradeOrBuild(typeof(House)))
                return typeof(TradeAi);
        }

        if (!builtHarbour)
        {
            //Build Harbour if none is Built yet
            if (UpgradeOrBuild(typeof(Harbour)))
            {
                builtHarbour = true;
                return typeof(TradeAi);
            }
        }

        Rock rock = Array.Find<Rock>(PlacementController.instance.rocks, r => r.team == mainbuilding.team && !r.occupied); ;
        if (rock != null)
        {
            if (UpgradeOrBuild(typeof(Mine)))
                return typeof(TradeAi);
        }

        return typeof(TradeAi);
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

    private bool UpgradeOrBuild(Type type)
    {
        int val = UnityEngine.Random.Range(0, 10);
        if (val < master.personality.building)
        {
            //Try Levelling Up
            foreach (var building in buildingList[type])
            {
                if (gold >= building.levelCost && building.LevelUp())
                    return false;
            }
        }
        else
        {
            //check if there is enough money to build new Building
            if (buildingList[type].Count > 0 && gold < buildingList[type][0].buildCost)
                return false;

            //Didnt Level up, so we need a new one
            return PlaceBuilding(type);
        }
        return false;

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