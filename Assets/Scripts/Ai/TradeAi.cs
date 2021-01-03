using System;
using UnityEngine;


public class TradeAi : BaseUtilityAi
{
    float nextAction = 0f;
    float tradeCD = 30;

    public bool builtHarbour = false;

    public TradeAi(AiMaster master) : base(master)
    {
    }

    public bool Trade(resource receiveResource)
    {
        //TODO check if need to build Mine to be able to trade for it
        /*
            Rock rock = Array.Find<Rock>(PlacementController.instance.rocks, r => r.team == mainbuilding.team && !r.occupied); ;
            if (rock != null)
            {
                if (UpgradeOrBuild(typeof(Mine)))
                    return goal.INCREASE_STONE;
            }
        */
        foreach (Trade trade in TradeManager.instance.tradeToElementMapping.Keys)
        {
            if (trade.fromTrader.resource != receiveResource) continue;

            if (CanAcceptTrade(trade))
            {
                TradeManager.instance.AcceptTrade(trade, resourceManager);
                nextAction = Time.time + tradeCD;
                return true;
            }
        }
        return false;
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
        //cant take Trade since no harbour for ship  - - - - - - TODO ohne Find!
        else if (trade.type == tradeType.ship && !builtHarbour)//mainbuilding.buildings.Find(t => t.GetType() == typeof(Harbour)) == null)
        {
            //Couldnt accept trade since Harbour not built
            return false;
        }

        return true;
    }
}

