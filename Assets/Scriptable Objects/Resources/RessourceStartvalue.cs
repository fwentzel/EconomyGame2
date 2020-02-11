using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class RessourceStartvalue : ScriptableObject
{
	//serves as variable for curves used in eg loyaltychange calculation
	public List<Resource> startValues;


	//private void OnEnable()
	//{
	//	foreach (var item in startValues)
	//	{
	//		Debug.Log(item.name);
	//	}
	//}
}
