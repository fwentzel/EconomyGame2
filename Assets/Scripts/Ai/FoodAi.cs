
using System;
using UnityEngine;

public class FoodAi : BaseAi
{
    public FoodAi(AiMaster master) : base(master)
    {
    }

    public override goal Tick()
    {
        // lower Multiplier

        //Build / Upgrade Farm
        int foodChange = resourceManager.CalculateFoodGenerated() - resourceManager.GetAmount(resource.citizens) * mainbuilding.foodPerDayPerCitizen;

        if (resourceManager.foodChange <= -5 || resourceManager.isLoyaltyDecreasing)
        {
            if (!master.buildingAi.UpgradeOrBuild(typeof(Farm)))
                return goal.INCREASE_GOLD;
        }

        return goal.INCREASE_FOOD;
    }
}
