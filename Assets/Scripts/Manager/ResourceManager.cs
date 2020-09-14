using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class ResourceManager : MonoBehaviour
{
    public ResourceStartvalue resourceStartvalue;
    Dictionary<resource, int> resourceAmount;
    public float foodLoyaltyChange { get; private set; }
    public Mainbuilding mainbuilding;
    public Curve foodRatioToLoyaltyChange;
    public event Action OnResourceChange = delegate { };

    public bool isLoyaltyDecreasing = false;

    private void Awake()
    {
        PopulateRessourceAmounts();
    }

    private void Start()
    {
        GameManager.instance.OnCalculateIntervall += CalculateNextDay;
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
        if (mainbuilding.gameOver)//TODO HACKY. methode überlegen sie aus dem spiel zu nehmen
            return;

        CalculateGold();
        CalculateFood();
        CalculateStone();
        CalculateLoyalty();
        CompareToMeanCityResources();
        CheckGameOver();
        OnResourceChange?.Invoke();
    }

    private void CheckGameOver()
    {
        if (resourceAmount[resource.citizens] <= 0)
        {
            MessageSystem.instance.Message(string.Format("TEAM {0} IS ABANDONDED BY ITS CITIZENS...", mainbuilding.team), Color.red);
            //print("GAME OVER FOR PLAYER " + mainbuilding.team);
            mainbuilding.gameOver = true;
        }
    }

    private void CompareToMeanCityResources()
    {
        float diff = resourceAmount[resource.loyalty] - CityResourceLookup.instance.meanLoyalty;

        if (diff > 0)
        {
            //can pickup any free ciziens
            int random = Random.Range(1, 100);
			//random <= diff &&
            if ( resourceAmount[resource.citizens] < mainbuilding.maxCitizens)
            {
                CityResourceLookup.instance.TakeCitizen(this);
            }
        }
        else if (diff < -5)
        {
            //possibility that Citizens wander off
            int random = Random.Range(-100, -20);
            if (random <= diff)
            {
                CityResourceLookup.instance.LooseCitizen(this);
            }
        }
    }

    private void CalculateGold()
    {
        resourceAmount[resource.gold] += mainbuilding.Taxes * resourceAmount[resource.citizens];
    }

    private void CalculateFood()
    {
        int foodAmount = resourceAmount[resource.food];
        int foodChange = CalculateFoodChange();
        foodAmount += foodChange;
        if (foodAmount < 0)
        {
            foodAmount = 0;
            print(mainbuilding.team + " doesnt have any Food Left!");
        }

        resourceAmount[resource.food] = foodAmount;
    }


    public int CalculateFoodChange()
    {
        return CalculateFoodGenerated() - resourceAmount[resource.citizens] / mainbuilding.foodUsePerDayPerCitizen;
    }

    public int CalculateFoodGenerated()
    {
        int generatedAmount = 0;
        foreach (Building building in mainbuilding.buildings)
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
        foreach (Building building in mainbuilding.buildings)
        {
            Mine mine;
            if (mine = building as Mine)
            {
                generatedAmount += mine.unitsPerIntervall;
            }
        }
        resourceAmount[resource.stone] += generatedAmount;
    }

    private void CalculateLoyalty()
    {
        float newLoyalty = resourceAmount[resource.loyalty];
        float oldLoyalty = newLoyalty;
        int citizens = resourceAmount[resource.citizens];
        if (citizens > 0)
        {
            //Evaluate animation curve to determine loyalty Change based on foodunits per citizens
            float t = (resourceAmount[resource.food] * mainbuilding.foodUsePerDayPerCitizen) / citizens;
            foodLoyaltyChange = foodRatioToLoyaltyChange.curve.Evaluate(t);

            if (mainbuilding.maxCitizens / citizens < .5f)
                newLoyalty -= 5;
        }
        else
            newLoyalty -= 10;

        //taxes in Range (0,10). taxes= 5 results in neutral loyaltychange
        newLoyalty += 5 - mainbuilding.Taxes;

        newLoyalty += foodLoyaltyChange;

        if (newLoyalty > 100)
            newLoyalty = 100;
        if (newLoyalty <= 0)
        {
            newLoyalty = 0;
        }
        resourceAmount[resource.loyalty] = (int)newLoyalty;
        isLoyaltyDecreasing = newLoyalty < oldLoyalty;
    }

    public void ChangeRessourceAmount(resource res, int amount)
    {
        resourceAmount[res] += amount;
        OnResourceChange();
    }

    internal int GetAmount(resource resource)
    {
        return resourceAmount[resource];
    }

    internal String GetAmountUI(resource resource)
    {
        if (resource == resource.citizens)
            return GetAmount(resource) + "/" + mainbuilding.maxCitizens;

        return resourceAmount[resource].ToString();
    }
}
public enum resource
{
    food,
    loyalty,
    gold,
    citizens,
    stone
}