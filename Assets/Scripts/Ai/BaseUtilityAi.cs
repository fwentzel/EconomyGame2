
using UnityEngine;

public abstract class BaseUtilityAi
{
    public Mainbuilding mainbuilding;
    public ResourceManager resourceManager;
    public AiMaster master;

    Brain brain;

    public BaseUtilityAi(AiMaster master)
    {
        this.master = master;
        brain = master.brain;
        mainbuilding = master.mainbuilding;
        resourceManager = mainbuilding.resourceManager;
    }

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
