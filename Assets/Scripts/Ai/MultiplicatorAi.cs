using System;
using UnityEngine;

public class MultiplicatorAi : BaseUtilityAi
{
    int baseTax = 0;

    public MultiplicatorAi(int goldRaiseTaxesThresholdBase, int lowerTaxesLoyaltyThresholdBase, AiMaster master) : base(master)
    {
        baseTax = master.personality.moneyPriority;
    }


    public void ChangeTax()
    {
        if (resourceManager.isLoyaltyDecreasing)
            mainbuilding.Taxes -= 1;
        else if (mainbuilding.Taxes <= baseTax)
        {
            mainbuilding.Taxes = baseTax;
        }
    }

    public void ChangeFood()
    {
        throw new NotImplementedException();
    }




}
