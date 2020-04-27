using UnityEngine;
using System.Collections;

[System.Serializable]
public class Player : MonoBehaviour
{

	public Mainbuilding mainbuilding;
	public int team { get; private set; }
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
			UiManager.instance.CurrentRessouceManagerToShow = mainbuilding.resourceManager;
			CameraController camControl = FindObjectOfType<CameraController>();
			camControl.FocusOnMainBuilding(mainbuilding.transform.position);
		}
	}
	
	
}
