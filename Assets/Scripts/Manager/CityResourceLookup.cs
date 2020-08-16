using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class CityResourceLookup : MonoBehaviour
{
    public static CityResourceLookup instance { get; private set; }
    public GameObject citizenPrefab;
    public float meanLoyalty { get; private set; } = 50;
    public float freeCitizens { get; private set; } = 0;

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

   
    public void PopulateResourceManagers()
    {
        Player[] players = GameManager.instance.players;
        resourceManagers = new ResourceManager[players.Length];

        for (int i = 0; i < resourceManagers.Length; i++)
        {
            resourceManagers[i] = players[i].mainbuilding.resourceManager;
        }
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
        //print(string.Format("Team {0} hat einen Bürger aufgenommen!", resourceManager.mainbuilding.team));
        freeCitizens--;
        resourceManager.ChangeRessourceAmount(resource.citizens, 1);
    }

    internal void LooseCitizen(ResourceManager resourceManager)
    {
        //print(string.Format("Team {0} hat einen Bürger verloren!", resourceManager.mainbuilding.team));
        freeCitizens++;
        resourceManager.ChangeRessourceAmount(resource.citizens, -1);
        Instantiate(citizenPrefab, resourceManager.transform.position, Quaternion.identity);
    }
}
