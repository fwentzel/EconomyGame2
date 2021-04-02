using System;
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
        resourceManager.mainbuilding.maxCitizens += 4;
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

    protected override void SetupPossiblePlacements(Team t)
    {
        Vector3 tempMainPos = Array.Find(CitysMeanResource.instance.resourceManagers, resourceManager => resourceManager.mainbuilding.team == t).transform.position;
        Vector3Int mainBuildingPos = new Vector3Int((int)tempMainPos.x, (int)tempMainPos.y, (int)tempMainPos.z);
        int maxPlaceRange = PlacementController.instance.maxPlacementRadius;
        for (int x = mainBuildingPos.x - maxPlaceRange; x <= mainBuildingPos.x + maxPlaceRange; x++)
        {
            for (int z = mainBuildingPos.z - maxPlaceRange; z <= mainBuildingPos.z + maxPlaceRange; z++)
            {
                Vector2 pos = new Vector2(x, z);
                float dist = Vector3.Distance(mainBuildingPos, new Vector3(x, 0, z));
                if (PlacementController.instance.CheckSurroundingTiles(pos, 0, h => h == 0) && dist <= maxPlaceRange
                )
                {
                    if (dist == 0)
                        continue;
                    possiblePlacementsCache.Add(new Vector2(x, z));
                }
            }
        }
        base.SetupPossiblePlacements(t);
    }
    void GenerateNewCitizens(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (!CitizenManager.instance.HasFreeCitizens())
                return;
            Citizen citizen = CitizenManager.instance.GetFreeCitizen();
            citizen.Init(this, 1f, 1f);
            ChangeCitizenAmount(1, citizen);
        }
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
            citizen.Init(this);
        }
        if (i < newCitizens.Count)
            newCitizens = newCitizens.GetRange(i, newCitizens.Count - i);
        else
            newCitizens.Clear();
        return newCitizens;
    }

}
