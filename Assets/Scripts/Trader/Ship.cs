using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using UnityEngine;

public class Ship : MonoBehaviour
{
	public Curve heightCurve;
	public Team forTeam;
	[SerializeField] float speed = 1;
	[SerializeField] float wiggledampening = 3;
	[SerializeField] float turnSpeed;

	Animator animator;
	float startTime;

	public BGCurve curve;
	public BGCcMath math;
	private float distance = 0;

	state currentState = state.driving;
	int currentWaypoint = 0;


	void Awake()
	{
		animator = transform.GetChild(0).GetComponent<Animator>();
		transform.position = new Vector3(transform.position.x, 0, transform.position.z);
	}
	private void Start()
	{
		curve.AddPoint(new BGCurvePoint(curve, transform.position, BGCurvePoint.ControlTypeEnum.BezierSymmetrical), 0);
		startTime = Time.time;
		//rotate around y towards first point
		Vector3 tangent;
		Vector3 lookPos = math.CalcPositionAndTangentByDistance(.01f, out tangent);
		transform.rotation = Quaternion.LookRotation(tangent);

		//set animation Tri
		animator.SetTrigger("Emerge");
		TradeManager.instance.generator.waterMaterial.SetFloat("StartTime", startTime);
	}

	// Update is called once per frame
	void Update()
	{
		if (currentState == state.driving)
		{
			Move();
		}
		else if (currentState == state.reachedDestionation)
		{
			print("reached");
		}

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
			currentState = state.reachedDestionation;
	}

	private void IdleSineHeight()
	{


	}

	enum state
	{
		waiting,
		driving,
		reachedDestionation
	}
}