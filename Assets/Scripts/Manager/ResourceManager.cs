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
    public int foodChange { get; private set; }
    public bool canTakeCitizen { get; private set; }
    public List<Citizen> citizens = new List<Citizen>();

    [SerializeField] int cooldown = 5;
    int lastInteraction = -1;

    private void Awake()
    {
        resourceAmount = new Dictionary<resource, int>();
        foreach (var item in resourceStartvalue.resourceStartValues)
        {
            resourceAmount[item.Key.resource] = item.Value;
        }
    }

    private void Start()
    {
        GameManager.instance.OnCalculateIntervall += CalculateNextDay;
    }

    private void CalculateNextDay()
    {
        if (mainbuilding.gameOver)//TODO HACKY. methode überlegen sie aus dem spiel zu nehmen
            return;

        CalculateGold();
        CalculateFood();
        CalculateStone();
        CalculateLoyalty();
        // CitizenManagement();
        CheckCanTakeCitizen();
        CheckGameOver();
        OnResourceChange?.Invoke();
    }

    private void CheckCanTakeCitizen()
    {
        int newCitizenAmount = resourceAmount[resource.citizens] + 1;
        canTakeCitizen = resourceAmount[resource.food] > mainbuilding.defaultFoodPerDayPerCitizen * newCitizenAmount
                            && newCitizenAmount < mainbuilding.maxCitizens;
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

    private void CalculateGold()
    {
        float totalMultiplier = 0;
        for (int i = 0; i < citizens.Count; i++)
        {
            totalMultiplier += citizens[i].taxesMultiplier;
        }
        resourceAmount[resource.gold] += Mathf.RoundToInt(totalMultiplier * mainbuilding.Taxes);
    }

    private void CalculateFood()
    {
        int amount = 0;
        for (int i = 0; i < citizens.Count; i++)
        {
            amount += citizens[i].foodPerDay;
        }
        foodChange = CalculateFoodGenerated() - amount;

        int newAmount = resourceAmount[resource.food] + foodChange;
        resourceAmount[resource.food] = newAmount > 0 ? newAmount : 0;
    }

    public int CalculateFoodGenerated()
    {
        int generatedAmount = 0;
        foreach (Building building in mainbuilding.buildings.FindAll(t => t.GetType() == typeof(Farm)))
        {
            Farm farm = building as Farm;
            generatedAmount += farm.unitsPerIntervall;

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

        //taxes in Range (0,20). taxes= 10 results in neutral loyaltychange
        newLoyalty += ((mainbuilding.maxTaxes / 2) - mainbuilding.Taxes) / 2;


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

    public void ReceiveCitizen(Citizen citizen)
    {
        ChangeRessourceAmount(resource.citizens, 1);
        citizens.Add(citizen);
    }

    public void LooseCitizen(Citizen citizen)
    {
        ChangeRessourceAmount(resource.citizens, -1);
        citizens.Remove(citizen);
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