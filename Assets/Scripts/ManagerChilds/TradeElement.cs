using UnityEngine;
using UnityEngine.UI;
public class TradeElement : MonoBehaviour
{
	public Image giveImage;
	public Text giveAmountText;

	public Image receiveImage;
	public Text receiveAmountText;

	public Text titleText;

	public Text tradeTypeText;
	
	public Button acceptButton;
	
	private int amount;

	Trade trade;



	internal void Init(Trade trade)
	{
		giveImage.sprite = trade.givenAway.sprite;
		giveAmountText.text = trade.givenAwayAmount.ToString();

		receiveImage.sprite = trade.received.sprite;
		receiveAmountText.text = trade.receiveAmount.ToString();

		titleText.text = "TRADE";

		tradeTypeText.text=trade.type.ToString();

		this.trade = trade;

		acceptButton.onClick.AddListener(delegate () { TradeAccepted(); });
	}

	public void TradeAccepted()
	{
		TradeManager.instance.AcceptTrade(trade,UiManager.instance.currentRessouceManagerToShow);
	}

}


