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
		Dictionary<Type, BaseAi> states = new Dictionary<Type, BaseAi>() {
			{ typeof(TaxesAi),new TaxesAi(100,20,this)},
			{ typeof(BuildingAi),new BuildingAi(this)},
			// { typeof(TradeVehicleAi),new TradeVehicleAi(this)},
			{ typeof(TradeAi),new TradeAi(.2f,this)}
		};
		
		GetComponent<StateMachine>().availableStates = states;
	}

}