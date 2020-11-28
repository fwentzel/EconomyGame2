using System;
using UnityEngine;

public class TaxesAi : BaseAi
{
    int baseTax=0;
    public TaxesAi(int goldRaiseTaxesThresholdBase, int lowerTaxesLoyaltyThresholdBase, AiMaster master) : base(master)
    {
        baseTax=master.personality.taxes;
    }


    public override Type Tick()
    {
        

        if (resourceManager.isLoyaltyDecreasing)
            mainbuilding.Taxes -= 1;
        else if(mainbuilding.Taxes<=baseTax){
            mainbuilding.Taxes=baseTax ;
        }
        return typeof(BuildingAi);

    }


}
