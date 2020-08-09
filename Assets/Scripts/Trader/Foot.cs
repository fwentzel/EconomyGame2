using UnityEngine;
using System.Collections;

public class Foot : TradeVehicle
{
	private void Start()
	{
		Unload(1);
	}

	protected override IEnumerator Unload(float timeBeforeUnload)
	{
		StartCoroutine(base.Unload(timeBeforeUnload));
		//TODO Pooling
		yield return null;
	}

}

