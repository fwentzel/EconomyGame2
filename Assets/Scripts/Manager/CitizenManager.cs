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

    public void TakeOverCitizen(ResourceManager from, ResourceManager to, int amount)
    {
        List<Citizen> tradedCitizens = new List<Citizen>();
        int leftOver = amount;
        House house = null;
        foreach (Building building in from.mainbuilding.buildings.FindAll(t => t.GetType() == typeof(House)))
        {
            house = building as House;
            tradedCitizens.AddRange(house.SetFreeCitizens(leftOver));

            leftOver =amount- tradedCitizens.Count ;
            if (leftOver <= 0)
            {
                break;
            }
        }

        foreach (Citizen citizen in tradedCitizens)
        {
            citizen.foodMultiplier = 1.5f;
            citizen.taxesMultiplier = .5f;
        }

        foreach (Building building in to.mainbuilding.buildings.FindAll(t => t.GetType() == typeof(House)))
        {
            house = building as House;
            tradedCitizens = house.ReceiveCitizens(tradedCitizens);
            if (tradedCitizens.Count == 0)
            {
                break;
            }
        }
    }




}
