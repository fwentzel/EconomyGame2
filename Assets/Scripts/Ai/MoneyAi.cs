
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
        if (master.multiplicatorAi.ChangeTax())
        {

        }

        //Trade
        //TODO Komosch, dass hier gecheckt wird, ob hafen steht bzw hafen errichtet wird
        if (!master.tradeAi.builtHarbour)
        {
            //Some check
            if (master.buildingAi.UpgradeOrBuild(typeof(Harbour)))
            {
                master.tradeAi.builtHarbour = true;
                return goal.INCREASE_GOLD;//Trading
            }
        }

        if (master.tradeAi.Trade(resource.gold))

            return goal.INCREASE_GOLD;

        return goal.INCREASE_GOLD;
    }
}
