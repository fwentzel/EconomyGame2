﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 
public class TradeElement : MonoBehaviour
{
	[SerializeField] Image toTraderImage = null;
	[SerializeField] TMP_Text toTraderAmountText = null;

	[SerializeField] Image fromTraderImage = null;
	[SerializeField] TMP_Text fromTraderAmountText = null;

	[SerializeField] TMP_Text titleText = null;

	[SerializeField] TMP_Text tradeTypeText = null;

	[SerializeField] Button acceptButton = null;

	public ButtonCD buttonCD {get;private set;}
	public bool accepted { get; private set; } = false;
	public bool isOnCd { get; private set; } = false;

	Trade trade;
	int amount;
	float cd = 0;
	ResourceManager localRM;

	private void Awake()
	{
		acceptButton.onClick.AddListener(delegate () { TradeAccepted(); });
		buttonCD=acceptButton.GetComponent<ButtonCD>();
		buttonCD.OnCDFinished+=checkInteractable;
	}

	private void Start()
	{
		localRM = ResourceUiManager.instance.activeResourceMan;
		localRM.OnResourceChange += checkInteractable;
		
		checkInteractable();
	}

	private void Update()
	{
		if (isOnCd)
		{
			float t0 =  cd - TradeManager.instance.tradeCooldown;
			float amount = (Time.time - t0) / TradeManager.instance.tradeCooldown;
			acceptButton.image.fillAmount = amount;

			if (amount >= 1)
			{
				isOnCd = false;
				acceptButton.image.fillAmount = 1;
				checkInteractable();
			}
		}
	}

	internal void Init(Trade trade)
	{
		toTraderImage.sprite = trade.toTrader.sprite;
		toTraderAmountText.text = trade.toTraderAmount.ToString();

		fromTraderImage.sprite = trade.fromTrader.sprite;
		fromTraderAmountText.text = trade.fromTraderAmount.ToString();

		titleText.text = "TRADE";

		tradeTypeText.text = trade.type.ToString();
		this.trade = trade;

		EnableElement();
	}

	public void checkInteractable()
	{
		bool interactable = localRM.GetAmount(trade.toTrader.resource) > trade.toTraderAmount
							&& accepted == false
							&& TradeManager.instance.tradeCooldowns[localRM] <= Time.time;
		acceptButton.interactable = interactable;
	}


	public void DisableElement()
	{
		accepted = true;
		var colors = acceptButton.colors;
		colors.disabledColor = Color.red;
		acceptButton.colors = colors;
	}

	public void EnableElement()
	{
		accepted = false;
		var colors = acceptButton.colors;
		colors.disabledColor = Color.grey;
		acceptButton.colors = colors;
	}

	private void TradeAccepted()
	{
		MessageSystem.instance.Message("you accepted following Trade: "+ trade.ToString());
		ResourceManager rm = GameManager.instance.localPlayer.mainbuilding.resourceManager;
		TradeManager.instance.AcceptTrade(trade, rm);

		foreach (TradeElement elem in TradeManager.instance.tradeElements)
		{
			elem.buttonCD.SetUp(TradeManager.instance.tradeCooldown);
		}
	}

}


