using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class CitizenManager : MonoBehaviour
{
    public static CitizenManager instance { get; private set; }
    [SerializeField] GameObject citizenPrefab;

    [SerializeField]
    int totalAmountFreeCitizens = 1;
    [SerializeField]
    int LOOSE_CITIZEN_COOLDOWN_DAYS = 3;
    List<Citizen> citizens = new List<Citizen>();

    Dictionary<Team, int> looseCitizenCooldown = new Dictionary<Team, int>();


    private void Awake()
    {
        //singleton Check
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        SetupFreeCitizens();
    }
    private void Start()
    {
        GameManager.instance.OnGameStart += SetupCooldowns;
    }
    private void SetupCooldowns()
    {
        foreach (Player player in GameManager.instance.players)
        {
            looseCitizenCooldown.Add(player.team, 0);
        }
    }
    private void SetupFreeCitizens()
    {

        for (int i = 0; i < totalAmountFreeCitizens; i++)
        {
            citizens.Add(Instantiate(citizenPrefab, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<Citizen>());
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

            leftOver = amount - tradedCitizens.Count;
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

    public void FindNewHome(Citizen unhappyCitizen)
    {
        print(unhappyCitizen.team +"  "+(looseCitizenCooldown[unhappyCitizen.team] >= GameManager.instance.dayIndex));
        if (looseCitizenCooldown[unhappyCitizen.team] >= GameManager.instance.dayIndex)
            return;
        House oldHouse = unhappyCitizen.house;

        //Find new Home
        ResourceManager[] orderedResourceManagers = CitysMeanResource.instance.resourceManagers.OrderBy(t => t.GetAmount(resource.loyalty)).ToArray();
        for (int i = 0; i < orderedResourceManagers.Length; i++)
        {
            if (orderedResourceManagers[i].canTakeCitizen && unhappyCitizen.team != orderedResourceManagers[i].mainbuilding.team)
            {

                foreach (House house in orderedResourceManagers[i].mainbuilding.buildings.FindAll(t => t.GetType() == typeof(House)))
                {
                    //Add Citizen to new Home

                    if (house.ReceiveCitizen(unhappyCitizen))
                    {
                        //Remove Citizen from old home
                        oldHouse.ChangeCitizenAmount(-1, unhappyCitizen);
                        MessageSystem.instance.Message($"Citizen went from {oldHouse.team} to {house.team}");
                        looseCitizenCooldown[unhappyCitizen.team] = GameManager.instance.dayIndex + LOOSE_CITIZEN_COOLDOWN_DAYS;
                        break;
                    }
                }
            }
        }
    }

    public bool HasFreeCitizens()
    {
        return citizens.Count > 0;
    }

    public Citizen GetFreeCitizen()
    {
        Citizen citizen = citizens.First();
        citizens.Remove(citizen);
        return citizen;
    }

    public void SetCitizenFree(Citizen citizen)
    {
        citizens.Add(citizen);
    }


}
