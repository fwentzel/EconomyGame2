using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager instance { get; private set; }
	public static event Action OnNewDay=delegate { };

	[HideInInspector] public MainBuilding mainBuilding;
	public int dayLength=2;
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
		InvokeRepeating("InvokeNewDay", 0, GameManager.instance.dayLength);
	}
	private void InvokeNewDay()
	{
		OnNewDay();
	}
}
