using UnityEngine;
using System.Collections;

public class Foot : TradeVehicle
{
	private void Start()
	{
		StartCoroutine(UnloadCoroutine(1)); 
	}

	protected override IEnumerator UnloadCoroutine(float timeBeforeUnload)
	{
		StartCoroutine(base.UnloadCoroutine(timeBeforeUnload));
		//TODO Pooling
		yield return null;
	}

}

