using System;
using System.Collections.Generic;
using UnityEngine;

public class AiMaster: MonoBehaviour
{
	public Mainbuilding mainbuilding => GetComponent<Mainbuilding>();
	public StateMachine StateMachine => GetComponent<StateMachine>();

	public personality personality=personality.neutral;
	private void Awake()
	{
		InitializeStateMachine();
	}

	private void InitializeStateMachine()
	{
		var states = new Dictionary<Type, BaseAi>() {
			{ typeof(TaxesAi),new TaxesAi(100,20,this)},
			{ typeof(BuildingAi),new BuildingAi(this)},
			{ typeof(TradeVehicleAi),new TradeVehicleAi(this)},
			{ typeof(TradeAi),new TradeAi(.2f,this)}
		};
		GetComponent<StateMachine>().availableStates = states;
	}

}
public enum personality{
	safe,
	neutral,
	aggressive,
	emergency
}