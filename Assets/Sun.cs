using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
	private int dayLength;
	public float speedMult = 1;

	private void Start()
	{
		dayLength = GameManager.instance.dayLength;
	}
	void Update()
    {
		transform.RotateAround(Vector3.zero, Vector3.right, (180/dayLength) * Time.deltaTime );
		transform.LookAt(Vector3.zero);
    }
}
