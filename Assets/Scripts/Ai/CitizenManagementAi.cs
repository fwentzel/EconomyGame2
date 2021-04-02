
using System;
using UnityEngine;

public class CitizenManagementAi : BaseAi
{
    public CitizenManagementAi(AiMaster master) : base(master)
    {

    }

    public override GoalData Tick()
    {
        if (mainbuilding.maxCitizens- resAmount(resource.citizens) > 0 &&resAmount(resource.loyalty)<100)
        {
            return new GoalData(goal.INCREASE_LOYALTY, brain.GoalData.priority,true);
        }

        if (resourceManager.foodChange < 0 || resAmount(resource.food) < (resAmount(resource.citizens) * mainbuilding.foodPerDayPerCitizen))
            return new GoalData(goal.INCREASE_FOOD, brain.GoalData.priority,true);

        

        //Build more Homes to get new Citizen or Upgrade Home to get new Space
 
        if (!buildingAi.UpgradeOrBuild(typeof(House)))
            return new GoalData(goal.INCREASE_GOLD, brain.GoalData.priority,true);


        //Buy in Citizens


        return new GoalData(goal.HINDER_OTHERS, brain.GoalData.priority);
    }
}
