using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildingAi : BaseAi
{
	int maxMoneyThreshold = 500;
	Dictionary<Type, List<Building>> buildingList;
	List<Vector2> availableBuildSpots;
	int oldEnd;
	public BuildingAi(MainBuilding mainBuilding) : base(mainBuilding)
	{
		GetSpecificsFromBuilding();
		GetAvailableBuildSpots(1,2);
	}

	public override Type Tick()
	{
		int money = resourceManager.GetAmount(resource.money);
		if (resourceManager.CalculateFoodChange() < 0)
		{
			return UpgradeOrBuild(money, typeof(Farm));
		}
		if (resourceManager.GetAmount(resource.money) > maxMoneyThreshold)
		{
			//Act and Build 
			return UpgradeOrBuild(money, typeof(House));
		}

		return typeof(TradeAi);
	}

	private void GetAvailableBuildSpots(int start, int end)
	{
		if (start < 1)
			start = 1;
		Log(start + " " + end);
		oldEnd = end;
		availableBuildSpots = new List<Vector2>();
		Vector2 mainPos = new Vector2(mainBuilding.transform.position.x, mainBuilding.transform.position.z);
		for (int i = start; i < end; i++)
		{
			for (int x = -i; x <= i; x++)
			{
				for (int z = -i; z <= i; z++)
				{
					availableBuildSpots.Add(mainPos + new Vector2(x , z));
				}
			}
		}
		foreach (Building building in mainBuilding.buildings)
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
			{typeof(Farm),new List<Building>() }
		};

		foreach (Building building in mainBuilding.buildings)
		{
			buildingList[building.GetType()].Add(building);
		}
	}

	

	private Type UpgradeOrBuild(int money, Type type)
	{
		//Act and Build 
		foreach (var building in buildingList[type])
		{
			if (money < building.levelCost)
				continue;

			if (building.LevelUp())
				break;
		}

		if (money < buildingList[type][0].buildCost)
			return typeof(TradeAi);
		//Didnt Level up Farm, so we need a new one
		Log(" Added " + type.ToString());
		Vector3 pos = GetAvailablePosition();
		PlaceBuilding(type, pos);
		return typeof(TradeAi);
	}

	private void PlaceBuilding(Type type, Vector3 pos)
	{
		resourceManager.AddRessource(resource.money, -buildingList[type][0].buildCost);
		Building addedBuilding= mainBuilding.AddBuilding(type, pos);
		buildingList[type].Add(addedBuilding);
	}

	private Vector3 GetAvailablePosition()
	{
		if (availableBuildSpots.Count == 0)
			GetAvailableBuildSpots(oldEnd, oldEnd + 1);

		int index = 0;
		Vector3 pos = new Vector3(availableBuildSpots[index].x,PlacementController.instance.GetMeanHeightSurrounding(availableBuildSpots[index]), availableBuildSpots[index].y);
		availableBuildSpots.RemoveAt(index);
		return pos;
	}
}