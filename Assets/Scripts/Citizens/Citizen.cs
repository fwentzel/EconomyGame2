using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Citizen : MonoBehaviour
{

    public Team team;
    public float taxesMultiplier;
    public float foodMultiplier;
    public House house;
    Mainbuilding mainbuilding;
    public int happiness = 70;
    public int lookNewCityThreshold = 20;
    public int maxDiffHappinessToAllMeanLoyalty=10;

    public void Init(House house, float taxesMultiplier, float foodMultiplier)
    {
        this.house = house;
        this.taxesMultiplier = taxesMultiplier;
        this.foodMultiplier = foodMultiplier;
        team = house.team;
        mainbuilding = house.resourceManager.mainbuilding;
        happiness = mainbuilding.resourceManager.GetAmount(resource.loyalty);
    }
    public void Init(House house)
    {
        this.house = house;
        team = house.team;
        mainbuilding = house.resourceManager.mainbuilding;
        happiness = mainbuilding.resourceManager.GetAmount(resource.loyalty);
    }
    private void Start()
    {
        GameManager.instance.OnCalculateIntervall += UpdateHappiness;
    }

    private void UpdateHappiness()
    {
        if (house == null)
            return;
            
        if (taxesMultiplier > house.resourceManager.meanTaxesMultiplier)
            happiness -= 1;
        else if (taxesMultiplier < house.resourceManager.meanTaxesMultiplier)
            happiness += 1;
        if (foodMultiplier > house.resourceManager.meanFoodMultiplier)
            happiness += 1;
        else if (foodMultiplier < house.resourceManager.meanFoodMultiplier)
            happiness -= 1;

        if (mainbuilding.resourceManager.GetAmount(resource.food) == 0)
        {
            happiness -= 5;
        }

        //Main building settings
        happiness += Mathf.FloorToInt(((mainbuilding.maxTaxes / 2) - mainbuilding.Taxes) / 3f);
        happiness += (mainbuilding.foodPerDayPerCitizen - 2) * 2;

        happiness +=Mathf.FloorToInt((happiness- CitysMeanResource.instance.resourseMeanDict[resource.loyalty])/10);

        happiness = Mathf.Clamp(happiness, 0, 100);

        if (happiness < lookNewCityThreshold)
        {
            //Look for new City
            CitizenManager.instance.FindNewHome(this);
        }
    }
}




// NavMeshAgent agent;
// int walkRadius=10;
// [SerializeField] float speed=.4f;

// private void Start()
// {
// 	agent = GetComponent<NavMeshAgent>();
// 	agent.speed = speed;

// }
// private void Update()
// {
// 	if (agent.remainingDistance < .4f)
// 	{
// 		Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
// 		randomDirection += transform.position;
// 		NavMeshHit hit;
// 		NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
// 		Vector3 finalPosition = hit.position;
// 		agent.SetDestination(finalPosition);
// 	}
// }
