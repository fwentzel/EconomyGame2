using UnityEngine;
using System.Collections;
using Mirror;
using System.Collections.Generic;
using System;

public class MyNetworkManager : NetworkManager
{
	public static int connectedPlayers = 0;
	public static int maxHumanPlayers =1;

	public override void Awake()
	{
		base.Awake();
	}


	public override void OnServerAddPlayer(NetworkConnection conn)
	{
		connectedPlayers++;
		//First Assign Ai Players so Ready Command doesnt get sent before all Players are generated
		if (connectedPlayers == maxHumanPlayers)
		{
			FillPlayersWithAi();
		}
		SpawnPlayer(conn);
	}

	private void SpawnPlayer(NetworkConnection conn)
	{
		GameObject player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
		Player playerComponent = player.GetComponent<Player>();
		if (conn!=null)
		{
			NetworkServer.AddPlayerForConnection(conn, player);
		}
		else
		{
			playerComponent.isAi = true;
			NetworkUtility.instance.SpawnObject(player);
		}
		
		GameManager.instance.AddPlayer(playerComponent);
		playerComponent.RpcSetupPlayer();
	}

	public void FillPlayersWithAi()
	{
		for (int i = connectedPlayers; i < maxConnections; i++)
		{
			SpawnPlayer(null);
		}
		print("ALL PLAYERS ASSIGNED");
	}

	public override void OnServerDisconnect(NetworkConnection conn)
	{
		print("Player disconnected, replacing with AI");
		base.OnServerDisconnect(conn);
	}

}
