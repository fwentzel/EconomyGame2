
using System;
using UnityEngine;

public class LoyaltyAi : BaseAi
{
    public LoyaltyAi(AiMaster master) : base(master)
    {
    }

    public override goal Tick()
    {
        return goal.INCREASE_LOYALTY;
    }
}
