
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
            if ( UpgradeOrBuild(typeof(House)))
                return goal.INCREASE_CITIZENS;
        }


        //Buy in Citizens
        return goal.INCREASE_CITIZENS;
    }
}
int foodChange = resourceManager.CalculateFoodGenerated() - resourceManager.GetAmount(resource.citizens) * mainbuilding.foodPerDayPerCitizen;

if (resourceManager.foodChange <= -5 || resourceManager.isLoyaltyDecreasing)
{
    if (UpgradeOrBuild(typeof(Farm)))
        return goal.INCREASE_FOOD;
}



if (!builtHarbour)
{
    //Build Harbour if none is Built yet
    if (UpgradeOrBuild(typeof(Harbour)))
    {
        builtHarbour = true;
        return goal.INCREASE_MONEY;//Trading
    }
}

Rock rock = Array.Find<Rock>(PlacementController.instance.rocks, r => r.team == mainbuilding.team && !r.occupied); ;
if (rock != null)
{
    if (UpgradeOrBuild(typeof(Mine)))
        return goal.INCREASE_STONE;
}

return goal.INCREASE_FOOD;