using System;
using System.Collections.Generic;
using UnityEngine;

public class AiMaster : MonoBehaviour
{
    public Mainbuilding mainbuilding => GetComponent<Mainbuilding>();
    public StateMachine StateMachine => GetComponent<StateMachine>();
    public AiPersonality personality;

    public Brain brain { get; private set; }

    //Utility AIs
    public BuildingAi buildingAi { get; private set; }
    public TradeAi tradeAi { get; private set; }
    public MultiplicatorAi multiplicatorAi { get; private set; }


    private void Awake()
    {
        if (PersonalityManager.instance.personalities.Length > mainbuilding.team.teamID)
            personality = PersonalityManager.instance.personalities[mainbuilding.team.teamID];
        else
            personality = PersonalityManager.instance.defaultPersonality;

        brain = gameObject.AddComponent<Brain>();

        SetupUtilityAi();

        InitializeStateMachine();


    }

    private void SetupUtilityAi()
    {
        buildingAi = new BuildingAi(this);
        tradeAi = new TradeAi(this);
        multiplicatorAi = new MultiplicatorAi(this);
    }

    private void InitializeStateMachine()
    {
        Dictionary<goal, BaseAi> states = new Dictionary<goal, BaseAi>() {
            { goal.INCREASE_GOLD   ,new MoneyAi(this)},
            { goal.INCREASE_CITIZENS,new CitizenManagementAi(this)},
            { goal.HINDER_OTHERS    ,new TradeVehicleAi(this)},
            { goal.INCREASE_FOOD    ,new FoodAi(this)},
            { goal.INCREASE_LOYALTY ,new LoyaltyAi(this)}
        };
        GetComponent<StateMachine>().currentState = states[goal.INCREASE_CITIZENS];
        GetComponent<StateMachine>().SetStates(states, this);

    }

}