using Mirror;
using System;
using System.Collections;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
	public static GameManager instance;
	public event Action OnCalculateIntervall = delegate { };

	public int dayIndex = 0;
	public int calcResourceIntervall = 10;

	public SyncListUInt playerIDs = new SyncListUInt();
	public Player[] players;
	int readyClients = 0;

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
	public void RpcFillPlayers()
	{
		int i = 0;
		foreach (var id in playerIDs)
		{
			players[i] = NetworkIdentity.spawned[id].GetComponent<Player>();
			i++;
		}

	}

	[ClientRpc]
	public void RpcSetupMainBuildingPlayer()
	{
		MainBuilding[] mainBuildings = FindObjectsOfType<MainBuilding>();
		for (int i = 0; i < players.Length; i++)
		{
			foreach (MainBuilding mainBuilding in mainBuildings)
			{
				if (mainBuilding.team == i)
				{
					mainBuilding.SetupMainBuilding();
					players[i].SetMainBuilding(mainBuilding.GetComponent<NetworkIdentity>().netId);
					
				}
				
			}
		}

	}

	[ClientRpc]
	void RpcStartInvokeCalcIntervall()
	{
		InvokeRepeating("InvokeCalculateResource", calcResourceIntervall, calcResourceIntervall);
	}

	private void InvokeCalculateResource()
	{
		print("NEW DAY");
		OnCalculateIntervall();
		dayIndex++;
	}

	[Server]
	public void AddPlayer(Player player)
	{
		playerIDs.Add(player.GetComponent<NetworkIdentity>().netId);
	}

	[Server]
	IEnumerator StartGame()
	{
		print("SERVER STARTING GAME!");
		RpcFillPlayers();
		RpcSetupMainBuildingPlayer();
		FindObjectOfType<GameTimer>().StartTimer(calcResourceIntervall);
		print("waiting 5 sec");
		yield return new WaitForSeconds(5);
		CityResourceLookup.instance.PopulateResourceManagers();
		UiManager.instance.UpdateRessourceUI();
		RpcStartInvokeCalcIntervall();

	}

	[Server]
	public void OnClientReady()
	{
		readyClients += 1;
		if (readyClients == MyNetworkManager.maxHumanPlayers)
		{
			StartCoroutine(StartGame());
		}
	}
}
