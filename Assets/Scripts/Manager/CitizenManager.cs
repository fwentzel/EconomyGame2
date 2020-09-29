using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class CitizenManager : MonoBehaviour
{
    public static CitizenManager instance { get; private set; }
    public GameObject citizenPrefab;

    public Dictionary<int, List<Citizen>> freeCitizensPerTeam { get; private set; } = new Dictionary<int, List<Citizen>>();
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
        GameManager.instance.OnGameStart += SetupFreeCitizens;
    }

    private void SetupFreeCitizens()
    {
        for (int i = 1; i <= GameManager.instance.players.Length; i++)
        {
            freeCitizensPerTeam[i] = new List<Citizen>();
        }
    }

    internal void TakeCitizen(ResourceManager resourceManager)
    {
        //TODO HÄSSLICH doppelt und dreifach meinbuildung team und RemoveAt
        if (citizens.Count <= 0 || resourceManager.GetAmount(resource.citizens) == resourceManager.mainbuilding.maxCitizens)
            return;
        Citizen citizen = null;
        //Prioritze Free citizens from this mainbuilding
        if (freeCitizensPerTeam[resourceManager.mainbuilding.team].Count > 0)
        {
            citizen = freeCitizensPerTeam[resourceManager.mainbuilding.team][0];
            freeCitizensPerTeam[resourceManager.mainbuilding.team].RemoveAt(0);
        }
        else
        {
            for (int i = 1; i <= freeCitizensPerTeam.Count; i++)
            {
                if (freeCitizensPerTeam[i].Count > 0)
                {
                    citizen = freeCitizensPerTeam[i][0];
                    freeCitizensPerTeam[i].RemoveAt(0);
                    break;
                }
            }
        }

        string message = string.Format("Team {0} took up a citizen from team {1}!", resourceManager.mainbuilding.team, citizen.team);
        MessageSystem.instance.Message(message, Color.green);
        
        citizens.Remove(citizen);
        
        Destroy(citizen.gameObject);

        foreach (House house in resourceManager.mainbuilding.buildings.FindAll(x => x.GetType() == typeof(House)))
        {
            if (house.currentAmount < house.capacity)
            {
                house.ChangeCitizenAmount(1);
                break;
            }
        }
    }

    internal void LooseCitizen(ResourceManager resourceManager)
    {
        string message = string.Format("Team {0} lost a citizen!", resourceManager.mainbuilding.team);
        MessageSystem.instance.Message(message);

        foreach (House house in resourceManager.mainbuilding.buildings.FindAll(x => x.GetType() == typeof(House)))
        {
            if (house.currentAmount > 0)
            {
                house.ChangeCitizenAmount(-1);
                break;
            }
        }

        GameObject citizen = Instantiate(citizenPrefab, resourceManager.transform.position, Quaternion.identity);
        Citizen citizenComponent = citizen.GetComponent<Citizen>();
        citizen.GetComponent<Citizen>().team = resourceManager.mainbuilding.team;
        
        citizens.Add(citizenComponent);
        freeCitizensPerTeam[resourceManager.mainbuilding.team].Add(citizenComponent);
    }
}
