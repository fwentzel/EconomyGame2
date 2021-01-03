
using System;
using UnityEngine;

public class FoodAi : BaseAi
{
    public FoodAi(AiMaster master) : base(master)
    {
    }

    public override goal Tick()
    {
        return goal.INCREASE_FOOD;
    }
}
