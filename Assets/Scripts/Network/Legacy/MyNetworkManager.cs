﻿using UnityEngine;
using System.Collections;

using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

public class MyNetworkManager : MonoBehaviour
{
	public static MyNetworkManager instance { get; private set; }
	public static int maxHumanPlayers = 1;

	public int connectedPlayers = 0;
	[SerializeField] private GameObject playerPrefab=null;

	public static int maxConnections = 4;
	List<Player> players = new List<Player>();

	private void Awake()
	{
		if (instance == null)
			instance = this;
		else
			Destroy(this);

		
	}

	private void Start()
	{
		OnServerAddPlayer();
	}

	public void OnServerAddPlayer()
	{
		connectedPlayers++;
		//First Assign Ai Players so Ready Command doesnt get sent before all Players are generated
		if (connectedPlayers == maxHumanPlayers)
		{
			FillPlayersWithAi();
		}
		SpawnPlayer(true);

	}

	private void SpawnPlayer(bool isHuman)
	{
		GameObject player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
		Player playerComponent = player.GetComponent<Player>();
		if (isHuman == true)
		{
			print("added hiuman");
			if(GameManager.instance.localPlayer==null)
				GameManager.instance.localPlayer = playerComponent;
			//NetworkServer.AddPlayerForConnection(conn, player);
		}
		else
		{
			print("added ai");
			playerComponent.isAi = true;
		}


		players.Add(playerComponent);
		if (players.Count == maxConnections)
		{
			GameManager.instance.players = players.ToArray();
			GameManager.instance.StartGame();
		}
	}

	public void FillPlayersWithAi()
	{
		for (int i = connectedPlayers; i < maxConnections; i++)
		{
			SpawnPlayer(false);
		}
		print("ALL PLAYERS ASSIGNED");
	}


	public void PlayerDisconnet()
	{

	}
}
