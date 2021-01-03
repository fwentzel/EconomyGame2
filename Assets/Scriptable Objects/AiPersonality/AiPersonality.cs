using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObj/AiPersonality")]
public class AiPersonality : ScriptableObject
{


    [Range(0, 10)]
    public int citizenPriority = 10;
    [Range(0, 10)]
    public int moneyPriority = 5;

    [Range(0, 10)]
    public int foodPriority = 5;

    [Range(0, 10)]
    public int loyaltyPriority = 5;
    
    [Range(0, 10)]
    public int stonePriority = 5;
    [Range(0, 10)]
    public int hinderOtherPriority = 5;


}
