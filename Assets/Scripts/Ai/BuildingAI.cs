using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingAi : BaseAi
{
    Dictionary<Type, List<Building>> buildingList;
    int previousEnd;
    int gold;

    public BuildingAi(AiMaster master) : base(master)
    {
        GetSpecificsFromBuilding();

    }

    public override Type Tick()
    {
        gold = resourceManager.GetAmount(resource.gold);

        if (mainbuilding.buildings.Find(t => t.GetType() == typeof(Harbour)) == null)
        {
            //Build Harbour if none is Built yet
            UpgradeOrBuild(typeof(Harbour));
            return typeof(TradeAi);
        }
        int foodChange = resourceManager.CalculateFoodGenerated() - resourceManager.GetAmount(resource.citizens) * mainbuilding.foodPerDayPerCitizen;

        if (resourceManager.foodChange <= -5 || resourceManager.isLoyaltyDecreasing)
        {
            UpgradeOrBuild(typeof(Farm));
            return typeof(TradeAi);
        }

        if (CitizenManager.instance.freeCitizensPerTeam[resourceManager.mainbuilding.team.teamID].Count == 0
        && (resourceManager.foodChange >= 0
        || resourceManager.GetAmount(resource.food) > (resourceManager.GetAmount(resource.citizens) * mainbuilding.foodPerDayPerCitizen) * 2))//double the food that is needed for ctizens
        {
            //Act and Build 
            UpgradeOrBuild(typeof(House));
            return typeof(TradeAi);
        }
        Rock rock = Array.Find<Rock>(PlacementController.instance.rocks, r => r.team == mainbuilding.team && !r.occupied); ;
        if (rock != null)
        {
            UpgradeOrBuild(typeof(Mine));
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

    private void UpgradeOrBuild(Type type)
    {
        //Try Levelling Up
        foreach (var building in buildingList[type])
        {
            if (gold >= building.levelCost && building.LevelUp())
                return;
        }

        //check if there is enough money to build new Building
        if (buildingList[type].Count > 0 && gold < buildingList[type][0].buildCost)
            return;


        //Didnt Level up, so we need a new one
        PlaceBuilding(type);
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