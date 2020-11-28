using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObj/AiPersonality")]
public class AiPersonality : ScriptableObject
{
    [Range(0, 9), Tooltip("Attention to Trades")]
    public int trade =5;

    [Range(0, 9),Tooltip("Focus on upgrading (0) or building(9)")]
    public int building=5;

    [Range(0, 9),Tooltip("Taxes oriented towards loyalty (0) or money (9)")]
    public int taxes=5;

    [Range(0, 9),Tooltip("Attention to stop other Tradevehicles")]
    public int tradeVehicles=5;


}
