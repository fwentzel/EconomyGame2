using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingAi : BaseAi
{
    Dictionary<Type, List<Building>> buildingList;
    List<Vector2> availableBuildSpots;
    int previousEnd;
    int gold;

    public BuildingAi(AiMaster master) : base(master)
    {
        GetSpecificsFromBuilding();
        if (PlacementController.instance != null)
            GetAvailableBuildSpots(1, PlacementController.instance.maxPlacementRange);
        else
        {
            GetAvailableBuildSpots(1, 6);
        }
    }

    public override Type Tick()
    {
        gold = resourceManager.GetAmount(resource.gold);
        // int foodChange = resourceManager.CalculateFoodGenerated() - resourceManager.GetAmount(resource.citizens) * mainbuilding.foodPerDayPerCitizen;

        // if (resourceManager.foodChange <= -5 || resourceManager.isLoyaltyDecreasing)
        // {
        //     UpgradeOrBuild(typeof(Farm));
        //     return typeof(TradeAi);
        // }

        if (CitizenManager.instance.freeCitizensPerTeam[resourceManager.mainbuilding.team.teamID].Count == 0
        && (resourceManager.foodChange >= 0
        || resourceManager.GetAmount(resource.food) > (resourceManager.GetAmount(resource.citizens) * mainbuilding.foodPerDayPerCitizen) * 2))//double the food that is needed for ctizens
        {
            //Act and Build 
            UpgradeOrBuild(typeof(House));
            return typeof(TradeAi);
        }
        if (mainbuilding.buildings.Find(t => t.GetType() == typeof(Harbour)) == null)
        {
            //Build Harbour if none is Built yet
            UpgradeOrBuild(typeof(Harbour));
            return typeof(TradeAi);
        }


        ResourceObject rock = Array.Find<ResourceObject>(PlacementController.instance.resourceObjects, r => r.team == mainbuilding.team && !r.occupied); ;
        if (rock != null)
        {
            UpgradeOrBuild(typeof(Mine));
            return typeof(TradeAi);
        }
        return typeof(TradeAi);
    }

    private void GetAvailableBuildSpots(int start, int end)
    {
        if (start < 1)
            start = 1;
        previousEnd = end;
        availableBuildSpots = new List<Vector2>();
        Vector2 mainPos = new Vector2(mainbuilding.transform.position.x, mainbuilding.transform.position.z);
        for (int i = start; i < end; i++)
        {
            for (int x = -i; x <= i; x++)
            {
                for (int z = -i; z <= i; z++)
                {

                    availableBuildSpots.Add(mainPos + new Vector2(x, z));
                }
            }
        }
        //remove mainbuilding pos
        availableBuildSpots.Remove(mainPos);

        //TODO bisschen hacky
        foreach (Building building in mainbuilding.buildings)
        {
            Vector2 buildingPos = new Vector2(building.transform.position.x, building.transform.position.z);
            if (availableBuildSpots.Contains(buildingPos))
                availableBuildSpots.Remove(buildingPos);
        }
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
        Vector3 pos = GetPosition();
        if (pos != Vector3.zero)//if equal, no space left
            PlaceBuilding(type, pos);
    }

    private void PlaceBuilding(Type type, Vector3 pos)
    {
        Building addedBuilding = mainbuilding.AddBuilding(type, pos);
        buildingList[type].Add(addedBuilding);
    }
    private Vector3 GetPosition()
    {
        if (availableBuildSpots.Count == 0)
        {
            Debug.Log("AI CANT BUILD ANYMORE");
            return Vector3.zero;
            // GetAvailableBuildSpots(previousEnd, previousEnd + 1);
        }
        int index = 0;
        Vector3 pos = new Vector3(availableBuildSpots[index].x, PlacementController.instance.GetMeanHeightSurrounding(availableBuildSpots[index]), availableBuildSpots[index].y);
        availableBuildSpots.RemoveAt(index);
        return pos;
    }
    // private Vector3 GetAvailablePosition(Building building)
    // {


        

    // }
}