using System.Collections;
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
    [SerializeField] Color takenDisabledColor = Color.red;

    public ButtonCD buttonCD { get; private set; }
    public bool accepted { get; private set; } = false;

    Trade trade;
    ResourceManager localResourceManager;
    Color normalColor;

    private void Awake()
    {
        acceptButton.onClick.AddListener(delegate () { TradeAccepted(); });
        buttonCD = acceptButton.GetComponent<ButtonCD>();
        normalColor = acceptButton.image.color;

    }


    internal void Init(Trade trade, ResourceManager resourceManager)
    {
        toTraderImage.sprite = trade.toTrader.sprite;
        toTraderAmountText.text = trade.toTraderAmount.ToString();

        fromTraderImage.sprite = trade.fromTrader.sprite;
        fromTraderAmountText.text = trade.fromTraderAmount.ToString();

        titleText.text = "TRADE";

        tradeTypeText.text = trade.type.ToString();
        this.trade = trade;



        if (localResourceManager != resourceManager)
        {
            if (localResourceManager != null)
            {
                localResourceManager.OnResourceChange -= checkInteractable;
            }
            localResourceManager = resourceManager;
            localResourceManager.OnResourceChange += checkInteractable;
        }


        EnableElement();
    }

    public void checkInteractable()
    {
        acceptButton.interactable = accepted == false && localResourceManager?.GetAmount(trade.toTrader.resource) > trade.toTraderAmount;

        if (trade.type == tradeType.ship && localResourceManager?.mainbuilding.buildings.Find(t => t.GetType() == typeof(Harbour)) == null)
        {
            //if Player doesnt have harbour override interactable
            acceptButton.interactable = false;

        }
    }


    public void DisableElement()
    {
        accepted = true;
        acceptButton.image.color = takenDisabledColor;
        checkInteractable();
    }

    public void EnableElement()
    {
        accepted = false;
        acceptButton.image.color = normalColor;
        checkInteractable();
    }

    public void TradeAccepted()
    {
        MessageSystem.instance.Message("you accepted following Trade: " + trade.ToString());
        TradeManager.instance.AcceptTrade(trade, GameManager.instance.localPlayer.mainbuilding.resourceManager);

        foreach (TradeElement elem in TradeManager.instance.tradeElements)
        {
            elem.buttonCD.SetUp(TradeManager.instance.tradeCooldown, () => elem.checkInteractable());
        }
    }

}


