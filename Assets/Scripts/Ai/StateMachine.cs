using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public BaseAi currentState;
    public event Action<BaseAi> OnStateChanged;

     Dictionary<goal, BaseAi> availableStates;
    Brain brain;
    private void Awake()
    {
        if (GameManager.instance == null)
        {
            Debug.LogError("GAME MAMANGER NOT INSTANTIATED! Statemachine will not be started!");
            return;
        }
        GameManager.instance.OnGameStart += () => InvokeRepeating("StateMachineUpdate", 0, GameManager.instance.calcResourceIntervall);
    }

    public void SetStates(Dictionary<goal,BaseAi> newStates, AiMaster master){
        availableStates=newStates;
        brain=master.brain;
    }

    private void StateMachineUpdate()
    {
        //Perform Tick for Ai of current goal
        currentState.Tick();

        //Swap to new State accoridng to new goal
        SwitchToNewState();

    }

    private void SwitchToNewState()
    {

        currentState = availableStates[brain.goalData.goal];
        print("NEW STATE: " +currentState);
       // OnStateChanged?.Invoke(currentState);
    }
}
