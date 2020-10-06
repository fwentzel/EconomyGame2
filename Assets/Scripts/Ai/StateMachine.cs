using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
	public Dictionary<Type, BaseAi> availableStates;
	public BaseAi currentState;
	public event Action<BaseAi> OnStateChanged;
	private void Awake()
	{
		InvokeRepeating("StateMachineUpdate", 0, GameManager.instance.calcResourceIntervall);
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
		//TODO only for debugging when removing different states
		currentState =availableStates.ContainsKey(nextState)?availableStates[nextState]: availableStates.ElementAt(UnityEngine.Random.Range(0,availableStates.Count)).Value;
		
		OnStateChanged?.Invoke(currentState);
	}
}
