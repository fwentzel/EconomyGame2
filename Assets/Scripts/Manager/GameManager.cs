using Mirror;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
	public static GameManager instance;
	public event Action OnCalculateIntervall=delegate { };

	public int dayIndex =0;
	public bool isStarted = false;

	public int calcResourceIntervall =10;
	
	public SyncListUInt playerIDs = new SyncListUInt();
	public Player[] players;

	//Debugging 
	public bool showAiLog = false;

	private void Awake()
	{
		if (instance == null)
			instance = this;
		else
			Destroy(this);
		players = new Player[NetworkManager.singleton.maxConnections];
	}

	[ClientRpc]
	public void RpcFillPlayerClient()
	{
		int i = 0;
		foreach (var id in playerIDs)
		{
			players[i]= NetworkIdentity.spawned[id].GetComponent<Player>();
			i++;
		}
		
	}

	[ClientRpc]
	public void RpcFillMainBuildingArray()
	{
		MainBuilding[] mainBuildings = FindObjectsOfType<MainBuilding>();

		for (int i =0; i< playerIDs.Count;i++ )
		{
			foreach (MainBuilding mainBuilding in mainBuildings)
			{
				if (mainBuilding.team.teamID == i)
				{
					players[i].MainBuilding = mainBuilding;
				}
			}
		}

		foreach (MainBuilding mainBuilding in mainBuildings)
		{
			mainBuilding.SetupMainBuilding();
		}

	}



	[Server]
	private void InvokeCalculateResource()
	{
		OnCalculateIntervall();
		dayIndex++;
	}

	[Server]
	public void AddPlayer(Player player)
	{
		playerIDs.Add(player.GetComponent<NetworkIdentity>().netId);
	}

	[Server]
	public void StartGame()
	{
		print("STARTING GAME!");
		FindObjectOfType<MapGenerator>().SetupMap();
		RpcFillPlayerClient();
		RpcFillMainBuildingArray();
		FindObjectOfType<GameTimer>().StartTimer(calcResourceIntervall);
		CityResourceLookup.instance.PopulateResourceManagers();
		PlacementController.instance.SetupGridParameter();

		
		InvokeRepeating("InvokeCalculateResource", calcResourceIntervall, calcResourceIntervall);
		isStarted = true;
	}
}
