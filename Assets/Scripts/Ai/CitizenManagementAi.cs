
using System;
using UnityEngine;

public class CitizenManagementAi : BaseAi
{
    public CitizenManagementAi(AiMaster master) : base(master)
    {
       
    }

    public override goal Tick()
    {
        //Build more Homes to get new Citizen or 
        //Upgrade Home to get new Space
        if (CitizenManager.instance.freeCitizensPerTeam[resourceManager.mainbuilding.team.teamID].Count == 0
       && (resourceManager.foodChange >= 0
       || resourceManager.GetAmount(resource.food) > (resourceManager.GetAmount(resource.citizens) * mainbuilding.foodPerDayPerCitizen) * 2))//double the food that is needed for ctizens
        {
            //Act and Build 
            if ( master.buildingAi.UpgradeOrBuild(typeof(House)))
                return goal.INCREASE_CITIZENS;
        }


        //Buy in Citizens
        return goal.INCREASE_CITIZENS;
    }
}
