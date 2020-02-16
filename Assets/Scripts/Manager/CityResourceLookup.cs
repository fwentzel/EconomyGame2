using UnityEngine;
using System.Collections;
using System;

public class CityResourceLookup : MonoBehaviour
{
	public static CityResourceLookup instance { get; private set; }

	public float meanLoyalty { get; private set; } = 50;
	public float freeCitizens { get; private set; } =0;

	ResourceManager[] resourceManagers;

	private void Awake()
	{
		//singleton Check
		if (instance == null)
			instance = this;
		else
			Destroy(this);
		GameManager.OnCalculateIntervall += UpdateCityResourceMean;
		resourceManagers = FindObjectsOfType<ResourceManager>();
	}

	private void UpdateCityResourceMean()
	{
		int mean = 0;
		foreach (ResourceManager resourceManager in resourceManagers)
		{
			mean += resourceManager.GetAmount(resource.loyalty);
		}
		meanLoyalty = mean / resourceManagers.Length;
	}

	internal void TakeCitizen(ResourceManager resourceManager)
	{
		if (freeCitizens <= 0)
			return;
		print(string.Format("Team {0} hat einen Bürger aufgenommen!", resourceManager.mainBuilding.team.teamID));
		freeCitizens--;
		resourceManager.AddRessource(resource.citizens,1);
	}

	internal void LooseCitizen(ResourceManager resourceManager)
	{
		print(string.Format("Team {0} hat einen Bürger verloren!", resourceManager.mainBuilding.team.teamID));
		freeCitizens++;
		resourceManager.AddRessource(resource.citizens, -1);
	}
}
