using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXManager : MonoBehaviour
{
	public static VFXManager instance { get; private set; }
	public GameObject levelUpEffect;
	private void Awake()
	{
		//singleton Check
		if (instance == null)
			instance = this;
		else
			Destroy(this);
	}

	public void PlayEffect(GameObject effectPrefab, Vector3 pos)
	{
		//TODO Pooling=
		GameObject newEffect = Instantiate(effectPrefab, new Vector3(pos.x, effectPrefab.transform.position.y, pos.z), Quaternion.identity);
		Destroy(newEffect, 4);
		
	}
}
