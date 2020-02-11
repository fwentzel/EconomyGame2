using System;
using System.Collections.Generic;
using UnityEngine;

public class AiMaster: MonoBehaviour
{
	public MainBuilding mainBuilding => GetComponent<MainBuilding>();
	public StateMachine StateMachine => GetComponent<StateMachine>();

	private void Awake()
	{
		InitializeStateMachine();
	}

	private void InitializeStateMachine()
	{
		var states = new Dictionary<Type, BaseAi>() {
			{ typeof(LoyaltyAi),new LoyaltyAi(1,0,100,20,mainBuilding)},
			{ typeof(BuildingAi),new BuildingAi(mainBuilding)},
			{ typeof(TradeAi),new TradeAi(.1f,mainBuilding)}
		};
		GetComponent<StateMachine>().availableStates = states;
	}
}
