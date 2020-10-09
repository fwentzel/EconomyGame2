using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public class CitysMeanResource : MonoBehaviour
{
    public static CitysMeanResource instance { get; private set; }


    public ResourceManager[] resourceManagers { get; private set; }

    public Dictionary<resource, int> resourseMeanDict  { get; private set; }

    private void Awake()
    {
        //singleton Check
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        resourseMeanDict= new Dictionary<resource, int>();
        foreach (resource item in Enum.GetValues(typeof(resource)).Cast<resource>())
        {
            resourseMeanDict[item] = 0;
        }
    }

    private void Start()
    {
        GameManager.instance.OnCalculateIntervall += UpdateCityResourceMean;
        GameManager.instance.OnGameStart +=()=> PopulateResourceManagers(GameManager.instance.players.Length);
    }


    public void PopulateResourceManagers(int numManagers)
    {
        Player[] players = GameManager.instance.players;
        resourceManagers = new ResourceManager[numManagers];

        for (int i = 0; i < resourceManagers.Length; i++)
        {
            resourceManagers[players[i].team.teamID - 1] = players[i].mainbuilding.resourceManager;
        }
        UpdateCityResourceMean();
    }

    private void UpdateCityResourceMean()
    {
        int mean = 0;
        resource[] resources= resourseMeanDict.Keys.ToArray();
        foreach (resource res in resources)
        {
            mean = 0;
            foreach (ResourceManager resourceManager in resourceManagers)
            {
                mean += resourceManager.GetAmount(res);
            }
            resourseMeanDict[res] = mean / resourceManagers.Length;
        }
    }
}
