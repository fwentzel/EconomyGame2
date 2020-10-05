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
    public List<Citizen> citizens = new List<Citizen>();
    public int foodChange { get; private set; }
    public bool canTakeCitizen { get; private set; }
    public float meanTaxesMultiplier {get;private set;}= 0;
     public float  meanFoodMultiplier {get;private set;}= 0;

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
        // CalculateLoyalty();//TODO so you dont start with 0 loyalty
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
        canTakeCitizen = resourceAmount[resource.food] > mainbuilding.foodPerDayPerCitizen * newCitizenAmount
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
         meanTaxesMultiplier = 0;
        for (int i = 0; i < citizens.Count; i++)
        {
            meanTaxesMultiplier += citizens[i].taxesMultiplier;
        }
        resourceAmount[resource.gold] += Mathf.RoundToInt(meanTaxesMultiplier * mainbuilding.Taxes);
    }

    private void CalculateFood()
    {
       meanFoodMultiplier=0;
        for (int i = 0; i < citizens.Count; i++)
        {
            meanFoodMultiplier += citizens[i].foodMultiplier;
        }

        foodChange =(int) (CalculateFoodGenerated() - (meanFoodMultiplier*mainbuilding.foodPerDayPerCitizen));

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
        int loyalty =0;
        for (int i = 0; i < citizens.Count; i++)
        {
            loyalty += citizens[i].happiness;
        }
        loyalty /=citizens.Count;

        isLoyaltyDecreasing = loyalty < resourceAmount[resource.loyalty];
        resourceAmount[resource.loyalty] = loyalty;
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