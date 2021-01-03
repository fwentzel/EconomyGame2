using System;
using UnityEngine;
using Random = UnityEngine.Random;
public abstract class BaseAi
{
    public Mainbuilding mainbuilding;
    public ResourceManager resourceManager;

    public AiMaster master;

    public BaseAi(AiMaster master)
    {
        this.master = master;
        mainbuilding = master.mainbuilding;
        resourceManager = mainbuilding.resourceManager;
    }

    public abstract goal Tick();
    protected int resAmount(resource res)
    {
        return resourceManager.GetAmount(res);
    }

    protected void Log(string msg)
    {
        if (GameManager.instance.showAiLog)
            Debug.Log(mainbuilding.team + msg);
    }
}
