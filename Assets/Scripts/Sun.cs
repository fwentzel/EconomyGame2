using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
	private int dayLength;

	private void Start()
	{
		dayLength = GameManager.instance.calcResourceIntervall;
		//transform.rotation = Quaternion.Euler(0, 0, 0);
	}
	void Update()
    {
		transform.RotateAround(Vector3.zero, Vector3.right, (10/dayLength) * Time.deltaTime );
		transform.RotateAround(Vector3.zero, Vector3.up, (10 / dayLength) * Time.deltaTime);
		transform.LookAt(Vector3.zero);
    }
}
