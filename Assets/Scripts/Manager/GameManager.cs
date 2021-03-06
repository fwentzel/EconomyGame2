﻿
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
		Mainbuilding[] mainbuildings = FindObjectsOfType<Mainbuilding>();
		for (int i = 0; i < players.Length; i++)
		{
			foreach (Mainbuilding mainbuilding in mainbuildings)
			{
				if (mainbuilding.team == i)
				{
					mainbuilding.SetupMainBuilding();
					players[i].SetMainBuilding(mainbuilding);
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
		OnCalculateIntervall();
		dayIndex++;
	}
	
	public void StartGame()
	{
		print("SERVER STARTING GAME!");
		RpcSetupMainBuildingPlayer();
		PlacementController.instance.SetupGridParameter();
		TradeManager.instance.StartTradeOffer();
		BuildUi.instance.GenerateBuildMenu();
		RpcStartInvokeCalcIntervall();
	}
	
	
}
