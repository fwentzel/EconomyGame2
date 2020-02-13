using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager instance { get; private set; }
	public static event Action OnCalculateIntervall=delegate { };
	public static int dayIndex=0;

	[HideInInspector] public MainBuilding mainBuilding;
	public int calcResourceIntervall=10;
	public Team team;

	//Debugging 
	public bool showAiLog = false;

	private void Awake()
	{
		//singleton Check
		if (instance == null)
			instance = this;
		else
			Destroy(this);

		foreach (MainBuilding mainBuilding in FindObjectsOfType<MainBuilding>())
		{
			if (mainBuilding.team == team)
			{
				this.mainBuilding = mainBuilding;
				break;
			}
		}
	}
	private void Start()
	{
		UiManager.instance.currentRessouceManagerToShow= mainBuilding.resourceManager;
		InvokeRepeating("InvokeCalculateResource", 0, GameManager.instance.calcResourceIntervall);
	}
	private void InvokeCalculateResource()
	{
		OnCalculateIntervall();
		dayIndex++;
	}
}
