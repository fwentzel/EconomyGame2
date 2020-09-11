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

    List<Citizen> citizens = new List<Citizen>();

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
        string message = string.Format("Team {0} took up a citizen from team {1}!", resourceManager.mainbuilding.team, citizens[0].team);
        MessageSystem.instance.Message(message, Color.green);
        Destroy(citizens[0].gameObject);
        citizens.RemoveAt(0);
        freeCitizens--;
        foreach (House house in resourceManager.mainbuilding.buildings.FindAll(delegate (Building building){return building.GetType() == typeof(House);}))
        {
            if(house.currentAmount<house.capacity){
                house.ChangeCitizenAmount(1);
                break;
            }
        }
    }

    internal void LooseCitizen(ResourceManager resourceManager)
    {
        string message = string.Format("Team {0} lost a citizen!", resourceManager.mainbuilding.team);
        MessageSystem.instance.Message(message);
        freeCitizens++;
        
         foreach (House house in resourceManager.mainbuilding.buildings.FindAll(x => x.GetType() == typeof(House)))
        {
            if(house.currentAmount>0){
                house.ChangeCitizenAmount(-1);
                break;
            }
        }

        GameObject citizen = Instantiate(citizenPrefab, resourceManager.transform.position, Quaternion.identity);
        Citizen citizenComponent = citizen.GetComponent<Citizen>();
        citizen.GetComponent<Citizen>().team = resourceManager.mainbuilding.team;
        citizens.Add(citizenComponent);
    }
}
