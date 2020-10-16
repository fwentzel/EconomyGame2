using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXManager : MonoBehaviour
{
	public static GameObject levelUpEffect;


	public static void PlayEffect( Vector3 pos)
	{
		if(levelUpEffect== null) return;
		//TODO Pooling=
		GameObject newEffect = Instantiate(levelUpEffect, new Vector3(pos.x, levelUpEffect.transform.position.y, pos.z), Quaternion.identity);
		Destroy(newEffect, 4);
		
	}
}
