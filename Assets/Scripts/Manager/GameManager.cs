using Mirror;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager instance { get; private set; }
	public static event Action OnCalculateIntervall=delegate { };
	public static int dayIndex=0;

	[HideInInspector] public MainBuilding []mainBuildings;
	public int calcResourceIntervall=10;
	public Team[] teams;
	public Team team;
	public Player localPlayer;

	//Debugging 
	public bool showAiLog = false;


	private void Awake()
	{
		//singleton Check
		if (instance == null)
			instance = this;
		else
			Destroy(this);
	}

	internal MainBuilding GetMainbuildingByTeamID(int connectedPlayers)
	{
		MainBuilding mainBuilding = null;
		foreach (MainBuilding building in GameManager.instance.mainBuildings)
		{
			if (building.team.teamID == connectedPlayers)
			{
				mainBuilding = building;
			}
		}
		return mainBuilding;
	}

	private void Start()
	{
		mainBuildings = FindObjectsOfType<MainBuilding>();
		InvokeRepeating("InvokeCalculateResource", calcResourceIntervall, calcResourceIntervall);
	}


	private void InvokeCalculateResource()
	{
		OnCalculateIntervall();
		dayIndex++;
	}
}
