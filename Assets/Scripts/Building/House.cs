using System.Collections.Generic;
using UnityEngine;

public class House : Building
{
    public int capacity;
    public int currentAmount { get; private set; } = 0;

    List<Citizen> citizens = new List<Citizen>();

    public override void OnBuild(bool subtractResource = true)
    {
        resourceManager.mainbuilding.maxCitizens += capacity;
        GenerateNewCitizens(capacity);
        base.OnBuild(subtractResource);
    }

    public void ChangeCitizenAmount(int amount, Citizen citizen)
    {
        if (amount > 0)
        {
            citizens.Add(citizen);
            resourceManager.ReceiveCitizen(citizen);
        }
        else
        {
            citizens.Remove(citizen);
            resourceManager.LooseCitizen(citizen);
        }
        currentAmount += amount;
    }

    public override void DestroyBuilding()
    {
        for (int i = 0; i < currentAmount; i++)
        {
            ChangeCitizenAmount(-1, citizens[0]);
        }
        resourceManager.mainbuilding.maxCitizens -= capacity;
        base.DestroyBuilding();
    }
    public override string GetStats()
    {
        return $"House\nLevel  {level} \nCapacity: {currentAmount}/{capacity}";
    }
    protected override void TriggerBonusLevel()
    {
        capacity += 4;
        resourceManager.mainbuilding.maxCitizens +=4;
        GenerateNewCitizens(2);
    }

    protected override void OnLevelUp()
    {
        base.OnLevelUp();
        GenerateNewCitizens(1);
        capacity += 2;
        resourceManager.mainbuilding.maxCitizens += 2;
        //ResourceUiManager.instance.UpdateRessourceUI(resource.citizens);

    }

    void GenerateNewCitizens(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Citizen citizen =Instantiate(CitizenManager.instance.citizenPrefab,new Vector3(0,0,0),Quaternion.identity).GetComponent<Citizen>();
            citizen.Init(team, this, 1f, 1f);
            ChangeCitizenAmount(1, citizen);
        }
    }

    public List<Citizen> SetFreeCitizens(int amount)
    {
        List<Citizen> citizensSetFree = new List<Citizen>();

        for (int i = 0; i < amount; i++)
        {
            if (citizens.Count > 0)
            {
                Citizen citizen = citizens[0];
                citizensSetFree.Add(citizen);
                ChangeCitizenAmount(-1, citizen);
            }

            else break;
        }
        return citizensSetFree;
    }

    public List<Citizen> ReceiveCitizens(List<Citizen> newCitizens)
    {
        int i = 0;
        for (i = 0; i < newCitizens.Count; i++)
        {
            if (currentAmount == capacity)
                break;
            Citizen citizen = newCitizens[0];
            ChangeCitizenAmount(1, citizen);
        }
        if (i < newCitizens.Count)
            newCitizens = newCitizens.GetRange(i, newCitizens.Count-i);
        else
            newCitizens.Clear();
        return newCitizens;
    }

}
