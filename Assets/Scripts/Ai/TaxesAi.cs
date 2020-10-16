using System;
using UnityEngine;

public class TaxesAi : BaseAi
{
    int goldRaiseTaxesThresholdBase = 200;
    int lowerTaxesLoyaltyThresholdBase = 20;

    int currentGoldRaiseTaxesThreshold;
    int currentLowerTaxesLoyaltyThreshold;

    public TaxesAi(int goldRaiseTaxesThresholdBase, int lowerTaxesLoyaltyThresholdBase, AiMaster master) : base(master)
    {
        this.goldRaiseTaxesThresholdBase = goldRaiseTaxesThresholdBase;
        this.lowerTaxesLoyaltyThresholdBase = lowerTaxesLoyaltyThresholdBase;
    }


    public override Type Tick()
    {


        //second check so taxes dont skyrocket in 1 day, so wait for new goldcalculation
        if (resAmount(resource.gold) < goldRaiseTaxesThresholdBase)
        {
            Log("NEED MONEY");
            mainbuilding.Taxes += 1;

        }
        if (resourceManager.isLoyaltyDecreasing)
            mainbuilding.Taxes -= 2;

        return typeof(BuildingAi);

    }


}
