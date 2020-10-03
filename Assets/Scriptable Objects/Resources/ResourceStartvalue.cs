using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObj/ResourceStartvalue")]
public class ResourceStartvalue : ScriptableObject
{
	//serves as variable for curves used in eg loyaltychange calculation
	public List<Resource> resources;
	public DictionaryResourceInt resourceStartValues;

	[ContextMenu("Fill Dict")]
	public void fillDict(){
		resourceStartValues.Clear();
		foreach (var item in resources)
		{
			resourceStartValues[item]=item.defaultStartAmount;
			
		}
	}
	
}
