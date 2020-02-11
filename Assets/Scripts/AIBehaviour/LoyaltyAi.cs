using System;
using UnityEngine;

public class LoyaltyAi : BaseAi
{
	public float desiredfoodLoyaltyChange { get; private set; } = 3;
	int desiredTaxesLoyaltyChange = 0;
	int moneyRaiseTaxesThreshold = 200;
	int loyaltyRiskyThreshold = 20;

	public LoyaltyAi(float desiredfoodLoyaltyChange, int desiredTaxesLoyaltyChange, int moneyRaiseTaxesThreshold, int loyaltyRiskyThreshold, MainBuilding mainBuilding) :base(mainBuilding)
	{
		this.desiredfoodLoyaltyChange = desiredfoodLoyaltyChange;
		this.desiredTaxesLoyaltyChange = desiredTaxesLoyaltyChange;
		this.moneyRaiseTaxesThreshold = moneyRaiseTaxesThreshold;
		this.loyaltyRiskyThreshold = loyaltyRiskyThreshold;
	}


	public override Type Tick()
	{
		if (resAmount(resource.loyalty) > 90 )
		{
			Log("rising Taxes by 2!");
			if(mainBuilding.Taxes==10)
				return typeof(BuildingAi);
			mainBuilding.Taxes += 2;
			return typeof(LoyaltyAi);
		}
		if (resAmount(resource.loyalty) < loyaltyRiskyThreshold)
		{
			Log("DESPERATE FOR LOYALTY! 0 Taxes");
			mainBuilding.Taxes = 0;
			return typeof(LoyaltyAi);
		}

		if (resAmount(resource.loyalty) > 50 && resAmount(resource.money) < moneyRaiseTaxesThreshold)
		{
			Log("rising Taxes by 1! NEED MONEY");
			mainBuilding.Taxes += 1;
			return typeof(LoyaltyAi);
		}
		else if (resAmount(resource.loyalty) < 50 && resAmount(resource.money) > moneyRaiseTaxesThreshold)
		{
			Log("lowering Taxes! NEED LOYALTY");
			mainBuilding.Taxes -= 1;
			return typeof(LoyaltyAi);
		}
		else
		{
			Log("normal Taxes!");
			mainBuilding.Taxes = 5;
			return typeof(BuildingAi);
		}
	}


}
