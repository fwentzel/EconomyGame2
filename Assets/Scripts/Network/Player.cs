using UnityEngine;
using System.Collections;
using Mirror;
[System.Serializable]
public class Player : NetworkBehaviour
{

	public MainBuilding mainBuilding;
	public int team { get; private set; }
	[SyncVar]
	public bool isAi = false;

	public void SetMainBuilding(uint mainBuildingNetId)
	{
		mainBuilding = NetworkIdentity.spawned[mainBuildingNetId].GetComponent<MainBuilding>();
		print("SET MAIN BUILDING " + mainBuilding.ToString());
		team = mainBuilding.team;
		UiManager.instance.currentRessouceManagerToShow = mainBuilding.resourceManager;
	}

	[ClientRpc]
	public void RpcSetupPlayer()
	{
		print("Setting up Player!");
		if (!isAi)
			FindObjectOfType<MapGenerator>().SetupMap();
		if (hasAuthority)
		{
			PlacementController.instance.SetupGridParameter();
			CmdReady();
		}
	}

	[Command]
	public void CmdReady()
	{
		GameManager.instance.OnClientReady();
	}
}
