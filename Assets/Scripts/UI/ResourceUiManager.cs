﻿using UnityEngine;
using System.Collections;

public class ResourceUiManager : MonoBehaviour
{
	public static ResourceUiManager instance { get; private set; }
	public ResourceManager activeResourceMan { get => currentRessouceManagerToShow; set => SetRM(value); }

	private Color defaultTextColor;
	[SerializeField] RessourceStartvalue res = null;

	ResourceManager currentRessouceManagerToShow;
	// Use this for initialization
	private void Awake()
	{
		//singleton Check
		if (instance == null)
			instance = this;
		else
			Destroy(this);

		foreach (Resource resource in res.startValues)
		{
			resource.SearchUiDisplay();
		}
		defaultTextColor = res.startValues[0].uiDisplay.color;
	}

	public void UpdateRessourceUI()
	{
		if (currentRessouceManagerToShow == null)
			return;
		foreach (Resource res in res.startValues)
		{
			if (res.resource == resource.loyalty)
				res.uiDisplay.color = activeResourceMan.isLoyaltyDecreasing ? Color.red : defaultTextColor;
			res.uiDisplay.text = currentRessouceManagerToShow.GetAmountUI(res.resource);
		}
	}

	void SetRM(ResourceManager rm)
	{
		if (currentRessouceManagerToShow != null)
			//Stop listening to old RM Event
			currentRessouceManagerToShow.OnResourceChange -= UpdateRessourceUI;

		//Set new Manager and start listening to new Event
		currentRessouceManagerToShow = rm;
		currentRessouceManagerToShow.OnResourceChange += UpdateRessourceUI;
	}

}
