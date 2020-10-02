using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObj/ColorToObjectMapping")]
public class ColorToObjectMapping : ScriptableObject
{
	//serves as variable for curves used in eg loyaltychange calculation
	public ColorToObject[] colorObjectMappings;
}