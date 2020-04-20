using System;
using UnityEngine;

public abstract class BaseAi 
{
	public MainBuilding mainBuilding;
	public ResourceManager resourceManager;

	public BaseAi(MainBuilding mainBuilding)
	{
		this.mainBuilding = mainBuilding;
		resourceManager = mainBuilding.resourceManager;
	}
	

	public abstract Type Tick();
	protected int resAmount(resource res)
	{
		return resourceManager.GetAmount(res);
	}

	protected void Log(string msg)
	{
		if (GameManager.instance.showAiLog)
			Debug.Log(mainBuilding.team + msg);
	}
}
