using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Citizen 
{
    public Team team;
    public float taxesMultiplier;
    public int foodPerDay;
    public House house;

    public Citizen(Team team,House house, float taxesMultiplier, int foodPerDay)
    {
        this.team = team;
        this.house=house;
        this.taxesMultiplier = taxesMultiplier;
        this.foodPerDay = foodPerDay;
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
}
