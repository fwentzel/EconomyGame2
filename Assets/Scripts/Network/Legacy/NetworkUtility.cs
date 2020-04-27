using UnityEngine;
using System.Collections;


public class NetworkUtility : MonoBehaviour
{
	public static NetworkUtility instance { get; private set; }

	private void Awake()
	{
		//singleton Check
		if (instance == null)
			instance = this;
		else
			Destroy(this);
	}

	

}
