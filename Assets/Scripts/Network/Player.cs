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
		print(mainBuildingNetId);
		mainBuilding = NetworkIdentity.spawned[mainBuildingNetId].GetComponent<MainBuilding>();
		print("SET MAIN BUILDING " + mainBuilding.ToString());
		team = mainBuilding.team;
		if (isLocalPlayer)
		{
			UiManager.instance.currentRessouceManagerToShow = mainBuilding.resourceManager;
			UiManager.instance.UpdateRessourceUI();
			//Calculat new Z so mainbuilding is in focus
			Vector3 mainbuildingPos = mainBuilding.transform.position;
			//Get Cameracontroller from Localmanager Object. UiManager is on there aswell so we us that singleton
			CameraController camControl = UiManager.instance.GetComponent<CameraController>();
			camControl.FocusOnMainBuilding(mainbuildingPos);
		}
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
