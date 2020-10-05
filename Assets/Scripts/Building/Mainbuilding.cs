﻿
using System;
using System.Collections.Generic;
using UnityEngine;

public class Mainbuilding : Building
{
	private int taxes = 10;
	public int defaultFoodPerDayPerCitizen = 2;
	[SerializeField]
	public List<Building> buildings { get; private set; }
	public Dictionary<Type, GameObject> possibleBuildings;
	public int Taxes { get => taxes; set => taxes = Mathf.Clamp(value, 0, maxTaxes); }
	public bool gameOver=false;
	public int maxCitizens = 0;

	public int maxTaxes = 20;

	private void Awake()
	{
		resourceManager = GetComponent<ResourceManager>();
		resourceManager.mainbuilding = this;
	}

	public void SetupMainBuilding(bool isAi)
	{
		PopulateBuildungs();
		if (isAi)
		{
			//Add AI Elements
			gameObject.AddComponent<StateMachine>();
			gameObject.AddComponent<AiMaster>();
		}
	}

	private void PopulateBuildungs()
	{
		buildings = new List<Building>();
		foreach (Building building in FindObjectsOfType<Building>())
		{
			if (building.team == team && building != this)
			{
				AddBuilding(building, subtractFromResource:false);
			}
		}

		possibleBuildings = new Dictionary<Type, GameObject>();
		foreach (ColorToObject cto in FindObjectOfType<MapGenerator>().colorToObjectMapping.colorObjectMappings)
		{
			Building building;
			if ((building = cto.placeable.GetComponent<Building>()) != null)
			{
				possibleBuildings[building.GetType()] = cto.placeable;
			}
		}

	}

	public void AddBuilding(Building building,bool subtractFromResource=true)
	{
		buildings.Add(building);
		building.resourceManager = resourceManager;
		building.OnBuild(subtractFromResource);
		building.team = team;
		building.enabled = true;
	}

	//Ai version
	public Building AddBuilding(Type buildingType, Vector3 pos)
	{
		if (pos == Vector3.zero)
			pos = transform.position + new Vector3(2, 0, 2);
		//TODO SAME CODE AS IN MAPGENERATOR		
		Building building = Instantiate(possibleBuildings[buildingType], pos, Quaternion.identity).GetComponent<Building>();
		building.transform.rotation = MapGenerator.GetRotationFromNormalSurface(building.gameObject);
		building.GetComponent<Building>().SetLevelMesh();

		AddBuilding(building);
		return building;
	}


	public override string GetStats()
	{
		return "Mainbuilding";
	}

	public override bool IsSelectable(){
		return true;
	}

}
