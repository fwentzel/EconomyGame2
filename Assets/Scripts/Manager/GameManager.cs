
using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;
	public event Action OnCalculateIntervall = delegate { };

	public int dayIndex = 0;
	public int calcResourceIntervall = 10;
	
	public Player[] players;
	public Player localPlayer;

	//Debugging 
	public bool showAiLog = false;

	private void Awake()
	{
		if (instance == null)
			instance = this;
		else
			Destroy(this);
		players = new Player[MyNetworkManager.maxConnections];
		
	}

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
					players[i].SetMainBuilding(mainBuilding);
				}
				
			}
		}
		CityResourceLookup.instance.PopulateResourceManagers();
	}
	
	void RpcStartInvokeCalcIntervall()
	{
		InvokeRepeating("InvokeCalculateResource", 0, calcResourceIntervall);
		FindObjectOfType<GameTimer>().StartTimer(calcResourceIntervall);
	}

	private void InvokeCalculateResource()
	{
		print("NEW DAY");
		OnCalculateIntervall();
		dayIndex++;
	}
	
	public void StartGame()
	{
		print("SERVER STARTING GAME!");
		RpcSetupMainBuildingPlayer();
		PlacementController.instance.SetupGridParameter();
		TradeManager.instance.RpcStartTradeOffer();
		RpcStartInvokeCalcIntervall();
	}
	
	
}
