
using System;
using UnityEngine;

public class MoneyAi : BaseAi
{
    public MoneyAi(AiMaster master) : base(master)
    {

    }

    public override goal Tick()
    {
        //raise Taxes

        //Trade

       return goal.INCREASE_MONEY;
    }
}
