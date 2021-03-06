﻿using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class Resource : ScriptableObject
{
	public resource resource;
	public int defaultStartAmount;
	public Sprite sprite;
	public Text uiDisplay;
	
	public void SearchUiDisplay()
	{
		foreach (Transform child in GameObject.Find("CityResourcePanel").transform)
		{
			//First child hold icon
			if (child.childCount > 1)//workaround for Gametimer
			{
				if (child.GetChild(0).GetComponent<Image>().mainTexture.name.Equals(sprite.name))
				{
					//second child holds Text Component
					uiDisplay = child.GetChild(1).GetComponent<Text>();
				}
			}
		}
	}
}