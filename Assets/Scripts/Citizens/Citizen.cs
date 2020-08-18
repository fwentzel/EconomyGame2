using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Citizen : MonoBehaviour
{
	NavMeshAgent agent;
	int walkRadius=10;
	[SerializeField] float speed=.4f;

	public int team;

	private void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		agent.speed = speed;
		
	}
	private void Update()
	{
		if (agent.remainingDistance < .4f)
		{
			Vector3 randomDirection = Random.insideUnitSphere * walkRadius;
			randomDirection += transform.position;
			NavMeshHit hit;
			NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
			Vector3 finalPosition = hit.position;
			agent.SetDestination(finalPosition);
		}
	}
}
