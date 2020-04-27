using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
	public Dictionary<Type, BaseAi> availableStates;
	public BaseAi currentState;
	public event Action<BaseAi> OnStateChanged;
	private void Awake()
	{
		InvokeRepeating("StateMachineUpdate", 0, 1f);
	}

	private void StateMachineUpdate()
	{
		if (currentState == null)
		{
			currentState = availableStates[typeof(BuildingAi)];
		}
		var nextState = currentState.Tick();

		if (nextState != null && nextState != currentState.GetType())
		{
			SwitchToNewState(nextState);
		}
	}

	private void SwitchToNewState(Type nextState)
	{
		currentState = availableStates[nextState];
		OnStateChanged?.Invoke(currentState);
	}
}
