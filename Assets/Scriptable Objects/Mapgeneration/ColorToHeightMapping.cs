using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObj/ColorToHeightMapping")]
public class ColorToHeightMapping : ScriptableObject
{
	//serves as variable for curves used in eg loyaltychange calculation
	public ColorToHeight[] colorHeightMappings;
}