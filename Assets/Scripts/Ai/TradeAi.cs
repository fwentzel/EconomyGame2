using System;
using UnityEngine;


public class TradeAi : BaseAi
{
    float nextAction = 0f;
    float tradeCD = 30;

    public TradeAi(AiMaster master) : base(master)
    {
    }

    public override Type Tick()
    {
        foreach (Trade trade in TradeManager.instance.tradeToElementMapping.Keys)
        {
            if (CanAcceptTrade(trade))
            { //Berechnen wie viel Prozent der Ressourcen für diesen Trade abgeben werden. unter einem Threshold kann bedenkenlos getraded werden
                int val = UnityEngine.Random.Range(0,10);
                if (val < master.personality.trade)
                {
                    TradeManager.instance.AcceptTrade(trade, resourceManager);
                    nextAction = Time.time + tradeCD;
                    break;
                }
            }
        }
        return typeof(TaxesAi);
        // return typeof(TradeVehicleAi);
    }

    private bool CanAcceptTrade(Trade trade)
    {
        //Trade is no longer available 
        if (TradeManager.instance.tradeToElementMapping[trade].accepted)
        {
            return false;
        }
        //Tradecooldown still active 
        else if (Time.time < nextAction)
        {
            return false;
        }
        //cant take Trade since no harbour for ship
        else if (trade.type == tradeType.ship && mainbuilding.buildings.Find(t => t.GetType() == typeof(Harbour)) == null)
        {
            return false;
        }

        return true;
    }
}
