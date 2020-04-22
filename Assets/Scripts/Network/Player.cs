using UnityEngine;
using System.Collections;

[System.Serializable]
public class Player : MonoBehaviour
{

	public MainBuilding mainBuilding;
	public int team { get; private set; }
	public bool isAi = false;

	public void SetMainBuilding(MainBuilding mainBuilding)
	{
		this.mainBuilding = mainBuilding;
		team = mainBuilding.team;

		if (GameManager.instance.localPlayer == this)
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
	
	
}
