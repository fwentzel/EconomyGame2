using UnityEngine;
using UnityEngine.UI;
public class TradeElement : MonoBehaviour
{
	[SerializeField] Image toTraderImage = null;
	[SerializeField] Text toTraderAmountText = null;

	[SerializeField] Image fromTraderImage = null;
	[SerializeField] Text fromTraderAmountText = null;

	[SerializeField] Text titleText = null;

	[SerializeField] Text tradeTypeText = null;

	[SerializeField] Button acceptButton = null;

	private int amount;
	public bool accepted { get; private set; } = false;
	Trade trade;
	bool hasTrade = false;

	internal void Init(Trade trade)
	{
		toTraderImage.sprite = trade.toTrader.sprite;
		toTraderAmountText.text = trade.toTraderAmount.ToString();

		fromTraderImage.sprite = trade.fromTrader.sprite;
		fromTraderAmountText.text = trade.fromTraderAmount.ToString();

		titleText.text = "TRADE";

		tradeTypeText.text = trade.type.ToString();
		hasTrade = true;
		this.trade = trade;

		EnableElement();
		acceptButton.onClick.AddListener(delegate () { TradeAccepted(); });
	}

	private void Update()
	{
		if (!hasTrade)
			return;
		bool interactable = ResourceUiManager.instance.activeResourceMan.GetAmount(trade.toTrader.resource) > trade.toTraderAmount
							&& accepted == false
							&& TradeManager.instance.tradeCooldowns[ResourceUiManager.instance.activeResourceMan] <= Time.time;
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
		TradeManager.instance.AcceptTrade(trade, GameManager.instance.localPlayer.mainbuilding.resourceManager);
	}

}


