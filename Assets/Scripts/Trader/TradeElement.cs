using UnityEngine;
using UnityEngine.UI;
public class TradeElement : MonoBehaviour
{
	public Image toTraderImage;
	public Text toTraderAmountText;

	public Image fromTraderImage;
	public Text fromTraderAmountText;

	public Text titleText;

	public Text tradeTypeText;
	
	public Button acceptButton;
	
	private int amount;
	private bool accepted=false;
	Trade trade;

	internal void Init(Trade trade)
	{
		toTraderImage.sprite = trade.toTrader.sprite;
		toTraderAmountText.text = trade.toTraderAmount.ToString();

		fromTraderImage.sprite = trade.fromTrader.sprite;
		fromTraderAmountText.text = trade.fromTraderAmount.ToString();

		titleText.text = "TRADE";

		tradeTypeText.text=trade.type.ToString();

		this.trade = trade;

		acceptButton.onClick.AddListener(delegate () { TradeAccepted(); });
	}

	private void Update()
	{
		bool interactable = (UiManager.instance.currentRessouceManagerToShow.GetAmount(trade.toTrader.resource) > trade.toTraderAmount &&accepted==false);
		acceptButton.interactable = interactable;
	}

	public void TradeAccepted()
	{
		TradeManager.instance.AcceptTrade(trade,GameManager.instance.localPlayer.mainBuilding.resourceManager);
		accepted = true;

		var colors = acceptButton.colors;
		colors.disabledColor = Color.red;
		acceptButton.colors = colors;
	}

}


