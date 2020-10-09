using System;
using UnityEngine;

public class TradeAi : BaseAi
{
	float safePercentOfResource = .1f;
	float nextAction =0f;
	float tradeCD=30;

	public TradeAi(float safePercentOfResource,AiMaster master) : base(master)
	{
		this.safePercentOfResource = safePercentOfResource;
	}

	public override Type Tick()
	{
		foreach (Trade trade in TradeManager.instance.tradeToElementMapping.Keys)
		{
			//Trade is no longer available or Tradecooldown still active or cant take Trade since no harbour for ship, so move on to next one
			if (TradeManager.instance.tradeToElementMapping[trade].accepted ||
			Time.time>nextAction||
			(trade.type==tradeType.ship && mainbuilding.buildings.Find(t=>t.GetType()==typeof(Harbour))==null))
				continue;
			
			
			 //Berechnen wie viel Prozent der Ressourcen für diesen Trade abgeben werden. unter einem Threshold kann bedenkenlos getraded werden
			float percentOfResource = (float)trade.toTraderAmount / resourceManager.GetAmount(trade.toTrader.resource);
			if (percentOfResource < safePercentOfResource)
			{
				TradeManager.instance.AcceptTrade(trade, resourceManager);
				nextAction =Time.time+tradeCD;
			}
		}
		return typeof(TaxesAi);
		// return typeof(TradeVehicleAi);
	}
}
