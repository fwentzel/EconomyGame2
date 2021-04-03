
using System;
using UnityEngine;

public class CitizenManagementAi : BaseAi
{
    public CitizenManagementAi(AiMaster master) : base(master)
    {

    }

    public override GoalData Tick()
    {
        //Get space for new Citizens

        if (mainbuilding.maxCitizens - resAmount(resource.citizens) == 0 && !buildingAi.UpgradeOrBuild(typeof(House)))
            return new GoalData(goal.INCREASE_GOLD, brain.GoalData.priority, true);

        //We have space,so increase loyalty to not loose any citizens and attract others
        if (resAmount(resource.loyalty) < 100 && resourceManager.loyaltyChange < master.personality.loyaltyPriority / 2)
        {
            return new GoalData(goal.INCREASE_LOYALTY, brain.GoalData.priority, true);
        }

        if (resourceManager.foodChange < 0 || resAmount(resource.food) < (resAmount(resource.citizens) * mainbuilding.foodPerDayPerCitizen))
            return new GoalData(goal.INCREASE_FOOD, brain.GoalData.priority, true);


        return new GoalData(goal.HINDER_OTHERS, brain.GoalData.priority);
    }
}
