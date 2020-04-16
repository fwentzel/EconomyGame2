using UnityEngine;
using System.Collections;
using Mirror;
using System.Collections.Generic;
using System;

public class MyNetworkManager : NetworkManager
{
	public int connectedPlayers = 0;
	[Range(1, 4)] public int maxPlayers =2;


	public override void Awake()
	{
		base.Awake();
	}


	public override void OnServerAddPlayer(NetworkConnection conn)
	{
		print("NEW CONNECTED PLAYER");
		GameObject player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
		Player playerComponent = player.GetComponent<Player>();
		NetworkServer.AddPlayerForConnection(conn, player);
		GameManager.instance.AddPlayer(playerComponent);
		connectedPlayers++;
		if(connectedPlayers== maxPlayers)
		{
			FillPlayersWithAi();
		}
	}

	public void FillPlayersWithAi()
	{
		for (int i = connectedPlayers; i < maxConnections; i++)
		{
			print("Adding Ai Player");
			GameObject aiPlayer = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
			
			Player aiPlayerComponent = aiPlayer.GetComponent<Player>();
			aiPlayerComponent.isAi = true;

			NetworkUtility.instance.SpawnObject(aiPlayer);
			GameManager.instance.AddPlayer(aiPlayerComponent);
		}
		print("All Players Connected. Game starts.");
		GameManager.instance.StartGame();
	}

	public override void OnServerDisconnect(NetworkConnection conn)
	{
		print("Player disconnected, replacing with AI");
		base.OnServerDisconnect(conn);
	}

}
