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

    bool returnToPrevious=false;
    private void Awake()
    {
        if (GameManager.instance == null)
        {
            Debug.LogError("GAME MAMANGER NOT INSTANTIATED! Statemachine will not be started!");
            return;
        }
        GameManager.instance.OnGameStart += () => InvokeRepeating("StateMachineUpdate", 0, GameManager.instance.calcResourceIntervall);
    }

    public void SetStates(Dictionary<goal, BaseAi> newStates, AiMaster master)
    {
        availableStates = newStates;
        brain = master.brain;
    }

    private void StateMachineUpdate()
    {
        //Perform Tick for Ai of current goal
        brain.GoalData = currentState.Tick();
        //Swap to new State accoridng to new goal
        SwitchToNewState();

    }

    private void SwitchToNewState()
    {
        
        if (returnToPrevious&& !brain.GoalData.returnToPreviousGoal)
        {
            //next goal was finished, so go back to previous one
            brain.GoalData = brain.previousGoalData;
        }
        returnToPrevious=brain.GoalData.returnToPreviousGoal;
        currentState = availableStates[brain.GoalData.goal];
        print("NEW STATE: " + currentState +" ("+brain.GoalData.goal+")");
    }
}
