using System;
using UnityEngine;
using Random = UnityEngine.Random;
public abstract class BaseAi
{
    public Mainbuilding mainbuilding{ get; private set; }
    public ResourceManager resourceManager{ get; private set; }

    public AiMaster master{ get; private set; }
    //Utility AIs
    public BuildingAi buildingAi{ get; private set; }
     public TradeAi tradeAi{ get; private set; }
    public MultiplicatorAi multiplicatorAi{ get; private set; }

    public Brain brain{ get; private set; }



    public BaseAi(AiMaster master)
    {
        this.master=master;
        brain=master.brain;
        mainbuilding = master.mainbuilding;
        resourceManager = mainbuilding.resourceManager;
        this.buildingAi=master.buildingAi;
        this.tradeAi=master.tradeAi;
        this.multiplicatorAi=master.multiplicatorAi;
    }

    public abstract GoalData Tick();
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
