using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ResourceManager : MonoBehaviour
{
	public RessourceStartvalue resourceStartvalue;
	Dictionary<resource, int> resourceAmount;
	public float foodLoyaltyChange { get; private set; }
	public MainBuilding mainBuilding;
	public Curve foodRatioToLoyaltyChange;


	private void Awake()
	{
		PopulateRessourceAmounts();
		GameManager.OnCalculateIntervall += CalculateNextDay;
	}

	private void PopulateRessourceAmounts()
	{
		resourceAmount = new Dictionary<resource, int>();
		foreach (Resource resource in resourceStartvalue.startValues)
		{
			resourceAmount[resource.resource] = resource.defaultStartAmount;
		}
	}

	private void CalculateNextDay()
	{
		CalculateMoney();
		CalculateFood();
		CalculateStone();
		CalculateLoyalty();
		TryUpdateUi();

		CheckGameOver();
		CompareToMeanCityResources();
	}

	private void CheckGameOver()
	{
		if (resourceAmount[resource.citizens] <= 0)
		{
			print("GAME OVER FOR TEAM " + mainBuilding.team.teamID);
		}
	}

	private void CompareToMeanCityResources()
	{
		float diff = resourceAmount[resource.loyalty] - CityResourceLookup.instance.meanLoyalty;
		if(diff>0)
		{
			//can pickup any free ciziens
			if (Random.Range(0, 100) <= diff)
			{
				CityResourceLookup.instance.TakeCitizen(this);
			}
		}
		else
		{
			//possibility that Citizens wander off
			if (Random.Range (-100,0) <= diff )
			{
				CityResourceLookup.instance.LooseCitizen(this);
			}
		}
	}

	private void CalculateMoney()
	{
		resourceAmount[resource.money] += mainBuilding.Taxes * resourceAmount[resource.citizens];
	}

	private void CalculateFood()
	{
		int foodAmount = resourceAmount[resource.food];
		int foodChange = CalculateFoodChange();
		foodAmount += foodChange;
		if (foodAmount < 0) {
			foodAmount = 0;
			print(mainBuilding.team + " doesnt have any Food Left!");
		}
			
		resourceAmount[resource.food] = foodAmount;
	}


	public int CalculateFoodChange()
	{
		return CalculateFoodGenerated() - resourceAmount[resource.citizens] / mainBuilding.foodUsePerDayPerCitizen;
	}

	public int CalculateFoodGenerated()
	{
		int generatedAmount = 0;
		foreach (Building building in mainBuilding.buildings)
		{
			Farm farm;
			if (farm = building as Farm)
			{
				generatedAmount += farm.unitsPerIntervall;
			}
		}
		return generatedAmount;
	}

	private void CalculateStone()
	{
		int generatedAmount = 0;
		foreach (Building building in mainBuilding.buildings)
		{
			Mine mine;
			if (mine = building as Mine)
			{
				generatedAmount += mine.unitsPerIntervall;
			}
		}
		resourceAmount[resource.stone]+= generatedAmount;
	}

	private void CalculateLoyalty()
	{
		foodLoyaltyChange = 0;
		float newLoyalty = resourceAmount[resource.loyalty];
		//Evaluate animation curve to determine loyalty Change based on foodunits per citizens
		float t = (resourceAmount[resource.food] * mainBuilding.foodUsePerDayPerCitizen) / resourceAmount[resource.citizens];
		foodLoyaltyChange = foodRatioToLoyaltyChange.curve.Evaluate(t);

		//taxes in Range (0,10). taxes= 5 results in neutral loyaltychange
		newLoyalty += 5 - mainBuilding.Taxes;
		newLoyalty += foodLoyaltyChange;

		if (newLoyalty > 100)
			newLoyalty = 100;
		if (newLoyalty <= 0)
		{
			newLoyalty = 0;
		}
		resourceAmount[resource.loyalty] = (int)newLoyalty;
	}

	public void AddRessource(resource res,int amount)
	{
		resourceAmount[res] += amount;
		TryUpdateUi();
	}

	public void TryUpdateUi()
	{
		if (UiManager.instance != null && UiManager.instance.currentRessouceManagerToShow == this)
		{
			UiManager.instance.UpdateRessourceUI();
		}
		//TODO vll Woanders hin
			
	}
	internal int GetAmount(resource resource)
	{
		return resourceAmount[resource];
	}
}
public enum resource
{
	food,
	loyalty,
	money,
	citizens,
	stone
}