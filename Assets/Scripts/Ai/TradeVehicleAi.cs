using System;
using System.Collections.Generic;
using UnityEngine;

public class TradeVehicleAi : BaseAi
{
    public TradeVehicleAi(AiMaster master) : base(master)
    {

    }

    public override GoalData Tick()
    {
        
        return new GoalData(goal.INCREASE_CITIZENS, brain.GoalData.priority);
    }

}
