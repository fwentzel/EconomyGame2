
using System;
using UnityEngine;

public class FoodAi : BaseAi
{
    public FoodAi(AiMaster master) : base(master)
    {
    }


    public override GoalData Tick()
    {
        //Build / Upgrade Farm
        if (!buildingAi.UpgradeOrBuild(typeof(Farm)))
            //CAnt build because no money available. So 
            return new GoalData(goal.INCREASE_GOLD, brain.GoalData.priority,true);
        
        
        // lower Multiplier

        return new GoalData(goal.HINDER_OTHERS, brain.GoalData.priority);
    }
}
