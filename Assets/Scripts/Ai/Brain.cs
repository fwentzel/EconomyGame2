
using System;
using UnityEngine;

public class Brain : MonoBehaviour
{
    public GoalData goalData;
    public struct GoalData
    {
        public goal goal { get; private set; }

        public int priority { get; private set; }

        public void PutGoal(goal newTarget, int newPriority)
        {
            int clampedPrio = Mathf.Clamp(newPriority, 0, 10);
            if (clampedPrio <= priority)
            {
                print("Didnt update Goal since Prio is less or equal. Goal is still: " + goal);
                return;
            }
            goal = newTarget;
            priority = clampedPrio;
            print("Updated Goal to: " + goal);
        }
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