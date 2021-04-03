using System;
using UnityEngine;

public class MultiplicatorAi : BaseUtilityAi
{

    public MultiplicatorAi(AiMaster master) : base(master)
    {
    }


    public bool ChangeTax(int changeValue)
    {
        if(mainbuilding.Taxes==0 ||mainbuilding.Taxes==mainbuilding.maxTaxes){
            return false;
        }
        else
        {
            mainbuilding.Taxes+=changeValue;
            return true;
        }
    }

    public bool ChangeFood(int changeValue)
    {
         if(mainbuilding.foodPerDayPerCitizen==0){
            return false;
        }
        else
        {
            mainbuilding.Taxes+=changeValue;
            return true;
        }
    }




}
