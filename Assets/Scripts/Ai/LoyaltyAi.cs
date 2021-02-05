
using System;
using UnityEngine;

public class LoyaltyAi : BaseAi
{
    public LoyaltyAi(AiMaster master) : base(master)
    {
    }

    public override GoalData Tick()
    {
        if (master.personality.foodPriority >= master.personality.moneyPriority)
        {
            if (multiplicatorAi.ChangeTax(-1))
            {
                return new GoalData(goal.INCREASE_GOLD, brain.GoalData.priority);
            }
        }

        if (master.personality.foodPriority < master.personality.moneyPriority)
        {
           if (multiplicatorAi.ChangeFood(1))
            return new GoalData(goal.INCREASE_FOOD, brain.GoalData.priority);
        }

        return new GoalData(goal.INCREASE_GOLD, brain.GoalData.priority);
    }
}
