
using System;
using UnityEngine;

public class Brain : MonoBehaviour
{
    public GoalData goalData ;
    private void Awake() {
        goalData=new GoalData();
        goalData.PutGoal(goal.INCREASE_CITIZENS,5);
    }
}

public struct GoalData
{
    public goal goal { get; private set; }
    public int priority { get; private set; }

    public void PutGoal(goal newGoal, int newPriority)
    {
        int clampedPrio = Mathf.Clamp(newPriority, 0, 10);
        if (clampedPrio <= priority)
        {
            Debug.Log("Didnt update Goal since Prio is less or equal. Goal is still: " + goal);
            return;
        }
        goal = newGoal;
        priority = clampedPrio;
        Debug.Log("Updated Goal to: " + goal);
    }
}


public enum goal
{
    INCREASE_CITIZENS,
    INCREASE_FOOD,
    INCREASE_MONEY,
    INCREASE_LOYALTY,
    HINDER_OTHERS,
    INCREASE_STONE
}