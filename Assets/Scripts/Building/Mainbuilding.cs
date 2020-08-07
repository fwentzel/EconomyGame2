
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Mainbuilding : Building
{
	[Range(0, 10)]
	private int taxes = 5;
	public int foodUsePerDayPerCitizen = 2;
	[SerializeField]
	public List<Building> buildings { get; private set; }
	public Dictionary<Type, GameObject> possibleBuildings;
	public int Taxes { get => taxes; set => taxes = Mathf.Clamp(value, 0, 10); }
	public bool gameOver=false;
	public int maxCitizens = 0;

	private void Awake()
	{
		resourceManager = GetComponent<ResourceManager>();
		resourceManager.mainbuilding = this;
	}

	public void SetupMainBuilding()
	{
		PopulateBuildungs();
		if (GameManager.instance.players[team].isAi)
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
		foreach (ColorToObject cto in FindObjectOfType<MapGenerator>().colorObjectMappings)
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
		string stats = "Type: Mainbuilding" + " \nTeam: " + team;
		return stats;
	}

}
