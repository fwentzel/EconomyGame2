using UnityEngine;
using System.Collections;
using Mirror;

public class Player :MonoBehaviour
{
	
	private MainBuilding mainBuilding;
	public Team team { get; private set; }
	public bool isAi=false;
	public MainBuilding MainBuilding{get { return mainBuilding; } set{ SetMainBuilding(value); }}

	void SetMainBuilding(MainBuilding mainBuilding)
	{
		this.mainBuilding = mainBuilding;
		team = mainBuilding.team;
		UiManager.instance.currentRessouceManagerToShow = mainBuilding.resourceManager;
	}
}
