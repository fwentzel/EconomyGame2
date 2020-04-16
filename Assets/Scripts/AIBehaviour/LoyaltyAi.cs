using System;
using UnityEngine;

public class TaxesAi : BaseAi
{
	int moneyRaiseTaxesThreshold = 200;
	int loyaltyRiskyThreshold = 20;
	int lastTaxchangeDay =-1;
	public TaxesAi( int moneyRaiseTaxesThreshold, int loyaltyRiskyThreshold, MainBuilding mainBuilding) :base(mainBuilding)
	{
		this.moneyRaiseTaxesThreshold = moneyRaiseTaxesThreshold;
		this.loyaltyRiskyThreshold = loyaltyRiskyThreshold;
	}


	public override Type Tick()
	{
		//second check so taxes dont skyrocket in 1 day, so wait for new moneycalculation
		if (resAmount(resource.money) < moneyRaiseTaxesThreshold&&GameManager.instance.dayIndex!= lastTaxchangeDay)
		{
			Log("NEED MONEY");
			mainBuilding.Taxes += 1;
			return typeof(BuildingAi);
		}

		mainBuilding.Taxes = resAmount(resource.loyalty)/10;
		return typeof(BuildingAi);
		
	}


}
