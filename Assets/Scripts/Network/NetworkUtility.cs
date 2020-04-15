using UnityEngine;
using System.Collections;
using Mirror;

public class NetworkUtility : NetworkBehaviour
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

	public void SpawnObject(GameObject obj)
	{
		if (NetworkServer.active)
		{
			NetworkServer.Spawn(obj);
		}
	}


}
