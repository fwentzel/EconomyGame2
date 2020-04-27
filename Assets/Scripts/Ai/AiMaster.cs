using System;
using System.Collections.Generic;
using UnityEngine;

public class AiMaster: MonoBehaviour
{
	public Mainbuilding mainbuilding => GetComponent<Mainbuilding>();
	public StateMachine StateMachine => GetComponent<StateMachine>();

	private void Awake()
	{
		InitializeStateMachine();
	}

	private void InitializeStateMachine()
	{
		var states = new Dictionary<Type, BaseAi>() {
			{ typeof(TaxesAi),new TaxesAi(100,20,mainbuilding)},
			{ typeof(BuildingAi),new BuildingAi(mainbuilding)},
			{ typeof(TradeAi),new TradeAi(.1f,mainbuilding)}
		};
		GetComponent<StateMachine>().availableStates = states;
	}
}
