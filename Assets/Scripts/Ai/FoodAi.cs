
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



        return goal.INCREASE_FOOD;
    }
}
