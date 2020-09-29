using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class Foot : TradeVehicle
{
    bool landed = false;
    public float fallSpeed = 1;
   

    public override void SetUp(ResourceManager resourceManager, Trade trade)
    {
        base.SetUp(resourceManager, trade);
    }
    private void Start()
    {
        Vector2 offsetDir = new Vector2(Random.Range(-1, 1),Random.Range(-1, 1));
        offsetDir*=(PlacementController.instance.maxPlacementRange + 1)*offsetDir;
        transform.position = new Vector3(rm.transform.position.x + offsetDir.x, transform.position.y, rm.transform.position.z +offsetDir.y);
    }

    private void FixedUpdate()
    {
        if (!landed)
        {
            CheckIfLanded();
        }
        else
        {
            HandleNavMeshAgent();
        }
    }

    private void HandleNavMeshAgent()
    {
        if (!agent.hasPath)
            agent.SetDestination(rm.transform.position);

        float dist = agent.remainingDistance;
        if (dist != 0 && dist < 1f)
        {
            agent.isStopped = true;
            StartCoroutine(UnloadCoroutine(2));
        }
    }

    private void CheckIfLanded()
    {
        if (transform.position.y < .2f)
        {
            Destroy(GetComponent<Rigidbody>());
            Vector3 pos = transform.position;
            transform.position = new Vector3(pos.x, 0.5f, pos.z);
            landed = true;
            agent=GetComponent<NavMeshAgent>();
            agent.enabled = true;
        }
    }

    protected override IEnumerator UnloadCoroutine(float timeBeforeUnload)
    {
        StartCoroutine(base.UnloadCoroutine(timeBeforeUnload));
        //TODO Pooling
        yield return null;
    }

}

