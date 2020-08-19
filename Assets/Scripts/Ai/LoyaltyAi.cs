using System;
using UnityEngine;

public class TaxesAi : BaseAi
{
	int goldRaiseTaxesThreshold = 200;
	int loyaltyRiskyThreshold = 20;
	int lastTaxchangeDay =-1;
	public TaxesAi( int goldRaiseTaxesThreshold, int loyaltyRiskyThreshold, Mainbuilding mainbuilding) :base(mainbuilding)
	{
		this.goldRaiseTaxesThreshold = goldRaiseTaxesThreshold;
		this.loyaltyRiskyThreshold = loyaltyRiskyThreshold;
	}


	public override Type Tick()
	{
		//second check so taxes dont skyrocket in 1 day, so wait for new goldcalculation
		if (resAmount(resource.gold) < goldRaiseTaxesThreshold&&GameManager.instance.dayIndex!= lastTaxchangeDay)
		{
			Log("NEED MONEY");
			mainbuilding.Taxes += 1;
			return typeof(BuildingAi);
		}

		mainbuilding.Taxes = resAmount(resource.loyalty)/10;
		return typeof(BuildingAi);
		
	}


}
