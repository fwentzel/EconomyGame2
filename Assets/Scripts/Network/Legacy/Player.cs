using UnityEngine;
using System.Collections;

[System.Serializable]
public class Player : MonoBehaviour
{

	public Mainbuilding mainbuilding;
	public Team team { get; private set; }
	public bool isAi = false;

	//private void Start()
	//{
	//	print("PLAYER START");
	//	MyNetworkManager.instance.OnServerAddPlayer();
	//}

	public void SetMainBuilding(Mainbuilding mainbuilding)
	{
		this.mainbuilding = mainbuilding;
		team = mainbuilding.team;

		if (GameManager.instance.localPlayer == this)
		{
			ResourceUiManager.instance.activeResourceMan = mainbuilding.resourceManager;
			CameraController camControl = FindObjectOfType<CameraController>();
			camControl.MoveCamOverObjectAt(mainbuilding.transform.position);
		}
	}
	
	
}
