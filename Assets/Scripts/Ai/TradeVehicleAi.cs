using System;
using System.Collections.Generic;
using UnityEngine;

public class TradeVehicleAi : BaseAi
{
    public TradeVehicleAi(AiMaster master) : base(master)
    {

    }

    public override goal Tick()
    {

        return goal.HINDER_OTHERS;
    }

}
