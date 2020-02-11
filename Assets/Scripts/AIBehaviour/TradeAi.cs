using System;
using UnityEngine;

public class TradeAi : BaseAi
{
	float safePercentOfResource = .1f;

	public TradeAi(float safePercentOfResource,MainBuilding mainBuilding):base(mainBuilding)
	{
		this.safePercentOfResource = safePercentOfResource;
	}

	public override Type Tick()
	{
		foreach (Trade trade in TradeManager.instance.tradeElements.Keys)
		{
			if (!TradeManager.instance.tradeElements[trade].acceptButton.interactable)
				continue;//Trade is no longer available, so move on to next one

			//Berechnen wie viel Prozent der Ressourcen für diesen Trade abgeben werden. unter einem Threshold kann bedenkenlos getraded werden
			float percentOfResource = (float)trade.givenAwayAmount / resourceManager.GetAmount(trade.givenAway.resource);
			if (percentOfResource < safePercentOfResource)
			{
				Log("traded Safe");
				TradeManager.instance.AcceptTrade(trade, resourceManager);
				return typeof(LoyaltyAi);
			}
		}
		Log("nothing to Trade ");
		return typeof(BuildingAi);
	}
}
