
using System;
using UnityEngine;

public class LoyaltyAi : BaseAi
{
    public LoyaltyAi(AiMaster master) : base(master)
    {
    }

    public override goal Tick()
    {
        //Decrease Taxes

        //Increase Food Multiplier

        return goal.INCREASE_LOYALTY;
    }
}
