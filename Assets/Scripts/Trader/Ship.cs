﻿using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Ship : TradeVehicle
{
    public Curve heightCurve;

    public Image tradeResourceImage;

    [SerializeField] float speed = 1;
    [SerializeField] float turnSpeed;

    GameObject wayPointsParent = null;
    Animator animator;
    float startTime;
    bool reached = false;
    float distance = 0;
    MapGenerator generator;
    BGCurve curve;
    BGCcMath math;


    void Awake()
    {
        //Get Waypointcurve At child index from team. Has to be setupup correct in scene
        generator = FindObjectOfType<MapGenerator>();
        animator = transform.GetChild(0).GetComponent<Animator>();
		
        transform.position = new Vector3(generator.xSize/2, 0, generator.zSize/2);
        
    }

    public override void SetUp(ResourceManager resourceManager, Trade trade)
    {
        base.SetUp(resourceManager, trade);

        wayPointsParent = GameObject.Find("ShipWayPoints");
        Transform curveTransform = wayPointsParent.transform.GetChild(rm.mainbuilding.team);
        curve = curveTransform.GetComponent<BGCurve>();
        math = curveTransform.GetComponent<BGCcMath>();
        curve.AddPoint(new BGCurvePoint(curve, transform.position, BGCurvePoint.ControlTypeEnum.BezierSymmetrical), 0);

        //rotate around y towards first point
        Vector3 tangent;
        Vector3 lookPos = math.CalcPositionAndTangentByDistance(.01f, out tangent);
        transform.rotation = Quaternion.LookRotation(tangent);

        //set animation Tri
        animator.SetTrigger("Emerge");
		startTime = Time.time;
        generator.waterMaterial.SetFloat("StartTime", startTime);
        tradeResourceImage.sprite = trade.toTrader.sprite;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!reached && !isStopped)
            Move();
    }

    private void Move()
    {
        //increase distance
        distance += speed * Time.deltaTime;
        float t = Time.time - startTime;

        //calculate position and tangent
        Vector3 tangent;
        Vector3 splinePos = math.CalcPositionAndTangentByDistance(distance, out tangent);

        transform.position = new Vector3(splinePos.x, heightCurve.curve.Evaluate(t), splinePos.z);

        //rotate with the spline
        transform.rotation = Quaternion.LookRotation(tangent);

        if (distance >= math.GetDistance())
        {
            StartCoroutine(UnloadCoroutine(3));
            reached = true;
        }
    }

    protected override IEnumerator UnloadCoroutine(float timeBeforeUnload)
    {
        StartCoroutine(base.UnloadCoroutine(timeBeforeUnload));
        //TODO Pooling
        yield return null;

    }




}