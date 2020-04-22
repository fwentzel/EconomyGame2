using UnityEngine;
using System.Collections;

using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

public class MyNetworkManager : MonoBehaviour
{
	public static int connectedPlayers = 0;
	public static int maxHumanPlayers = 1;
	[SerializeField] private GameObject playerPrefab;

	public static int maxConnections = 4;
	List<Player> players = new List<Player>();
	private void Start()
	{
		OnServerAddPlayer();
	}
	//public void StartServer(bool addHost)
	//{
	//	//SceneManager.LoadScene("Level");
	//	if (addHost)

	//}

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
