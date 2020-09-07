using System;
using System.Collections.Generic;
using UnityEngine;

public class TradeVehicleAi : BaseAi
{
    public TradeVehicleAi(AiMaster master) : base(master)
    {

    }

    public override Type Tick()
    {

        return typeof(BuildingAi);
    }

}
