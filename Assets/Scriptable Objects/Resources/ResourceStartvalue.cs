using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObj/ResourceStartvalue")]
public class ResourceStartvalue : ScriptableObject
{
	//serves as variable for curves used in eg loyaltychange calculation
	public List<Resource> startValues;

}
