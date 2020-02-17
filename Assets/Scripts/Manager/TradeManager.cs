using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TradeManager : MonoBehaviour
{
	public static TradeManager instance { get; private set; }

	public Dictionary<Trade, TradeElement> tradeElements { get; private set; }
	public Resource[] tradingResources;
	public GameObject tradeUiPanel;
	public GameObject newTradeTimerObject;
	public GameObject tradeElementPrefab;
	public GameObject wayPointsParent;

	public GameObject shipPrefab;


	public MapGenerator generator { get; private set; }

	int acceptedTrades = 0;


	private void Awake()
	{
		//singleton Check
		if (instance == null)
			instance = this;
		else
			Destroy(this);
		generator = FindObjectOfType<MapGenerator>();
		tradeElements = new Dictionary<Trade, TradeElement>();

		StartCoroutine("AnnounceNewTrades",2f);

	}

	private void GenerateNewTrades(int amount)
	{
		if (tradeElements.Count > 0)
		{
			List<Trade> acceptedTrades=new List<Trade>();
			//Delete accepted Trades
			foreach (var element in tradeElements)
			{
				Destroy(tradeElements[element.Key].gameObject);
				acceptedTrades.Add(element.Key);
			}
			foreach (Trade trade in acceptedTrades)
			{
				tradeElements.Remove(trade);
			}

		}

		for (int i = 0; i < amount; i++)
		{
			TradeElement newTradeElement = Instantiate(tradeElementPrefab, tradeUiPanel.transform).GetComponent<TradeElement>();
			Trade newTrade = GenerateTrade();

			tradeElements[newTrade] = newTradeElement;
			newTradeElement.Init(newTrade);
		}
		acceptedTrades = 0;
	}

	private Trade GenerateTrade()
	{
		Trade newTrade = new Trade();

		newTrade.toTrader = tradingResources[Random.Range(0, tradingResources.Length)];
		newTrade.toTraderAmount = Random.Range(50, 100);
		newTrade.fromTrader = tradingResources[Random.Range(0, tradingResources.Length)];
		newTrade.fromTraderAmount = Random.Range(80, 130);
		newTrade.type = tradeType.ship;
		if (Random.Range(0, 1f) < .5f)
			newTrade.type = tradeType.foot;
		return newTrade;
	}

	public void AcceptTrade(Trade trade, ResourceManager rm)
	{
		rm.AddRessource(trade.toTrader.resource, -trade.toTraderAmount);
		if (trade.type == tradeType.ship)
		{
			SpawnShip(trade,rm);
		}

		acceptedTrades++;
		if (acceptedTrades ==4)
		{
			StartCoroutine("AnnounceNewTrades", 10f);
		}
	}

	private void SpawnShip(Trade trade,ResourceManager rm)
	{
		Ship ship = Instantiate(shipPrefab, new Vector3(generator.xSize / 2, 0, generator.zSize / 2), Quaternion.identity).GetComponent<Ship>();
		ship.trade = trade;
		ship.rm = rm;
		//Get Waypointcurve At child index from team. Has to be setupup correct in scene
		Transform curveTransform = wayPointsParent.transform.GetChild(rm.mainBuilding.team.teamID);
		ship.curve = curveTransform.GetComponent<BGCurve>();
		ship.math = curveTransform.GetComponent<BGCcMath>();
	}

	IEnumerator AnnounceNewTrades(float duration)
	{
		float normalizedTime = duration;
		UiManager.instance.newTradesTimerParent.SetActive(true);
		while (normalizedTime >= 0)
		{
			UiManager.instance.newTradesInText.text = Mathf.CeilToInt(normalizedTime) + " seconds!";
			UiManager.instance.newTradesInImage.fillAmount = normalizedTime / duration;
			normalizedTime -= Time.deltaTime;
			yield return null;
		}
		UiManager.instance.newTradesTimerParent.SetActive(false);
		GenerateNewTrades(6);
	}
}
public enum tradeType
{
	ship,
	foot,
	instant
}