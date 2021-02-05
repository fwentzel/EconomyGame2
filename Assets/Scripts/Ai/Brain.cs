
using System;
using UnityEngine;

public class Brain : MonoBehaviour
{
    private GoalData goalData;
    public GoalData previousGoalData { get; private set; }

    public GoalData GoalData { get => goalData; set => SetGoalData(value); }

    private ResourceManager resourceManager;

    private void SetGoalData(GoalData value)
    {
        if (value.goal != goalData.goal)
            previousGoalData = goalData;
        goalData = value;
    }

    private void Awake()
    {
        goalData = new GoalData(goal.INCREASE_CITIZENS, 5);
        resourceManager = GetComponent<AiMaster>().mainbuilding.resourceManager;
    }

    internal void CheckIfOverrideGoal()
    {
        int foodChange = resourceManager.CalculateFoodGenerated() - resourceManager.GetAmount(resource.citizens) * resourceManager.mainbuilding.foodPerDayPerCitizen;
        if (resourceManager.foodChange <= -5)
        {
            goalData = new GoalData(goal.INCREASE_FOOD, 5);
        }
    }
}

public struct GoalData
{

    public goal goal { get; private set; }
    public int priority { get; private set; }

    public bool returnToPreviousGoal { get; private set; }

    public GoalData(goal goal, int priority, bool returnToPreviousGoal = false)
    {
        this.goal = goal;
        this.priority = priority;
        this.returnToPreviousGoal = returnToPreviousGoal;
    }

}


public enum goal
{
    INCREASE_CITIZENS,
    INCREASE_FOOD,
    INCREASE_GOLD,
    INCREASE_LOYALTY,
    HINDER_OTHERS,
    INCREASE_STONE
}