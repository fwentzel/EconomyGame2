
using System;
using UnityEngine;

public class CitizenManagementAi : BaseAi
{
    public CitizenManagementAi(AiMaster master) : base(master)
    {
    }

    public override goal Tick()
    {
        return goal.INCREASE_CITIZENS;
    }
}
