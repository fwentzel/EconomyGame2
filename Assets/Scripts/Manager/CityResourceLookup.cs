using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class CityResourceLookup : MonoBehaviour
{
    public static CityResourceLookup instance { get; private set; }

    public float meanLoyalty { get; private set; } = 50;

    public ResourceManager[] resourceManagers { get; private set; }


    private void Awake()
    {
        //singleton Check
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        GameManager.instance.OnCalculateIntervall += UpdateCityResourceMean;
        
    }


    public void PopulateResourceManagers(int numManagers)
    {
        Player[] players = GameManager.instance.players;
        resourceManagers = new ResourceManager[numManagers];

        for (int i = 0; i < resourceManagers.Length; i++)
        {
            resourceManagers[i] = players[i].mainbuilding.resourceManager;
        }
        UpdateCityResourceMean();
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

    
}
