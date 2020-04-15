using UnityEngine;
using System.Collections;
using Mirror;

public class MyNetworkManager : NetworkManager
{
	public Team[] teams;
	public int connectedPlayers = 0;


	public override void OnServerAddPlayer(NetworkConnection conn)
	{
		print("NEW CONNECTED PLAYER");
		MainBuilding mainBuilding = GameManager.instance.GetMainbuildingByTeamID(connectedPlayers);
		GameObject player = (GameObject)Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
		Player playerComponent = player.GetComponent<Player>();
		playerComponent.mainBuilding = mainBuilding;
		NetworkServer.AddPlayerForConnection(conn, player);
		if (player.GetComponent<NetworkIdentity>().isLocalPlayer)
		{
			print("NEW LOCAL PLAYER");
			GameManager.instance.localPlayer = playerComponent;
			//UiManager.instance.currentRessouceManagerToShow = mainBuilding.resourceManager;
		}
		connectedPlayers++;
	}
}
