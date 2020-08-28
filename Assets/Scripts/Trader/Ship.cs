using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Ship : TradeVehicle
{
    public Curve heightCurve;
    public Image tradeResourceImage;
    Animator animator;
    float startTime;
    MapGenerator generator;

    Transform child => transform.GetChild(0);

    float t = 0;
    float maxT = 0;

    int waterOffset;

    Vector3 targetPos;
    void Awake()
    {
        //Get Waypointcurve At child index from team. Has to be setupup correct in scene
        generator = FindObjectOfType<MapGenerator>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        waterOffset = generator.waterVertexHeight;

        transform.position = new Vector3(generator.xSize / 2, waterOffset, generator.zSize / 2);


        child.transform.position = new Vector3(child.transform.position.x, -waterOffset, child.transform.position.z);

        int len = heightCurve.curve.length;
        maxT = heightCurve.curve.keys[len - 1].time;
    }
    private void Start()
    {
        Harbour harbour = rm.mainbuilding.buildings.Find(x => x.GetType() == typeof(Harbour)) as Harbour;
        targetPos = harbour.transform.GetChild(harbour.transform.childCount-1).position;
        transform.LookAt(targetPos);
        agent.enabled=true;
        
        // //Get closest destination near harbour
        // NavMeshHit hit;
		// 	if (NavMesh.SamplePosition(targetPos, out hit, 1.0f, NavMesh.AllAreas)) {
		// 		targetPos = hit.position;
		// 	}
            
        agent.destination = targetPos;
    }

    public override void SetUp(ResourceManager resourceManager, Trade trade)
    {
        base.SetUp(resourceManager, trade);

        //set animation Tri
        animator.SetTrigger("Emerge");
        startTime = Time.time;
        generator.waterMaterial.SetFloat("StartTime", startTime);
        tradeResourceImage.sprite = trade.toTrader.sprite;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        HandleHeightCurve();
        float dist=agent.remainingDistance;
        if (dist>0&&dist < .1f)
        {
            StartCoroutine(UnloadCoroutine(3));
        }

    }

    private void HandleHeightCurve()
    {
       
        t += Time.deltaTime;
        Vector3 pos = transform.position;
        child.transform.position = new Vector3(pos.x, heightCurve.curve.Evaluate(t), pos.z);
        
    }

    protected override IEnumerator UnloadCoroutine(float timeBeforeUnload)
    {
        StartCoroutine(base.UnloadCoroutine(timeBeforeUnload));
        //TODO Pooling
        yield return null;

    }




}