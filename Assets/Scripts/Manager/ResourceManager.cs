using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class ResourceManager : MonoBehaviour
{
    public ResourceStartvalue resourceStartvalue;
    Dictionary<resource, int> resourceAmount;

    public Mainbuilding mainbuilding;
    public Curve foodRatioToLoyaltyChange;
    public event Action OnResourceChange = delegate { };

    public bool isLoyaltyDecreasing = false;
    public int foodChange {get; private set;}

    [SerializeField] int numSafeDaysAfterLoss = 5;
    int lastCitizenLost = -1;

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
        CalculateFoodChange();
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

        if (diff > 10)
        {
            //can pickup any free ciziens
            //int random = Random.Range(1, 100);
            //random <= diff &&
            // if ( resourceAmount[resource.citizens] < mainbuilding.maxCitizens)
            // {

            CitizenManager.instance.TakeCitizen(this);
            // }
        }
        else if (diff < -5 && lastCitizenLost + numSafeDaysAfterLoss < GameManager.instance.dayIndex)
        {
            //possibility that Citizens wander off
            // int random = Random.Range(-100, -20);
            // if (random <= diff)
            // {
            CitizenManager.instance.LooseCitizen(this);
            lastCitizenLost = GameManager.instance.dayIndex;
            // }
        }
    }

    private void CalculateGold()
    {
        resourceAmount[resource.gold] += mainbuilding.Taxes * resourceAmount[resource.citizens];
    }

    private void CalculateFood()
    {
        int foodAmount = resourceAmount[resource.food];
        foodAmount += foodChange;
        if (foodAmount < 0)
        {
            foodAmount = 0;
        }
        resourceAmount[resource.food] = foodAmount;
    }


    public void CalculateFoodChange()
    {
        foodChange = CalculateFoodGenerated() - resourceAmount[resource.citizens] * mainbuilding.foodUsePerDayPerCitizen;
    }

    private int CalculateFoodGenerated()
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
        int newLoyalty = resourceAmount[resource.loyalty];
        int oldLoyalty = newLoyalty;
        int citizens = resourceAmount[resource.citizens];

        //Evaluate animation curve to determine loyalty Change based on foodunits per citizens
        float t = foodChange /(float)(mainbuilding.maxCitizens * mainbuilding.foodUsePerDayPerCitizen);

        t = citizens > 0 ? t : -10;
        // if (mainbuilding.GetComponent<StateMachine>() == null)
        //     print(t);
        newLoyalty += Mathf.RoundToInt(foodRatioToLoyaltyChange.curve.Evaluate(t));

        // if (  citizens/mainbuilding.maxCitizens < .5f)
        //     newLoyalty -= 5;


        //taxes in Range (0,10). taxes= 5 results in neutral loyaltychange
        newLoyalty += (5 - mainbuilding.Taxes)/2;


        if (newLoyalty > 100)
            newLoyalty = 100;
        if (newLoyalty <= 0)
            newLoyalty = 0;

        resourceAmount[resource.loyalty] = newLoyalty;
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