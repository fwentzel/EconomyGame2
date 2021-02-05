
using System;
using UnityEngine;

public class MoneyAi : BaseAi
{

    public MoneyAi(AiMaster master) : base(master)
    {

    }

    public override GoalData Tick()
    {
        //raise Taxes
        if (multiplicatorAi.ChangeTax(+1))
        {
            return new GoalData(goal.INCREASE_GOLD, brain.GoalData.priority);
        }
        //Trade

        if (!tradeAi.Trade(resource.gold))
        {
            goal goalForTradeResource=goal.INCREASE_GOLD;
            switch (tradeAi.BestResourceNeededForTrade(resource.gold))
            {
                case resource.stone:
                    goalForTradeResource = goal.INCREASE_STONE;
                    break;
                case resource.food:
                    goalForTradeResource = goal.INCREASE_FOOD;
                    break;
               
            }
            return new GoalData(goalForTradeResource, brain.GoalData.priority, returnToPreviousGoal: true);
        }





        return brain.previousGoalData;
    }
}
