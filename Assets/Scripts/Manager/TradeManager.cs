using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
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

	public event Action<int> OnGenerateNewTrades = delegate { };

	int acceptedTrades = 0;
	int maxTrades = 6;
	int synchronizedValues = 4;//Workaround to know how big the stepsize for listtransformation is
	int[,] randomTradeValues;

	private void Awake()
	{
		//singleton Check
		if (instance == null)
			instance = this;
		else
			Destroy(this);
		generator = FindObjectOfType<MapGenerator>();
		tradeElements = new Dictionary<Trade, TradeElement>();
		randomTradeValues = new int[maxTrades, synchronizedValues];
	}
	private void Start()
	{
		StartTradeOffer();
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
		NewTradeRandomNumbers();
		for (int i = 0; i < amount; i++)
		{
			TradeElement newTradeElement = Instantiate(tradeElementPrefab, tradeUiPanel.transform).GetComponent<TradeElement>();
			Trade newTrade = GenerateTrade(i);

			tradeElements[newTrade] = newTradeElement;
			newTradeElement.Init(newTrade);
		}
		acceptedTrades = 0;
	}

	private Trade GenerateTrade(int i)
	{
		// [0,4] , [50,100] , [0,4] 
		Trade newTrade = new Trade();
		
		newTrade.toTrader = tradingResources[randomTradeValues[i, 0]];
		newTrade.toTraderAmount = randomTradeValues[i, 1];

		newTrade.fromTrader = tradingResources[randomTradeValues[i, 2]];
		newTrade.fromTraderAmount = randomTradeValues[i, 1] - 30;

		newTrade.type =(tradeType) Enum.GetValues(typeof(tradeType)).GetValue(randomTradeValues[i,3]); ;
		
		return newTrade;
	}

	private List<int> GenerateNewRandomNumbers()
	{
		List<int> tradeRandoms = new List<int>();
		for (int i = 0; i < maxTrades; i++)
		{
			int toTrader = Random.Range(0, tradingResources.Length); 
			int toTraderAmount = Random.Range(50, 100);
			int fromTrader = Random.Range(0, tradingResources.Length); ;
			int type = Random.Range(0, 2); ;

			tradeRandoms.Add(toTrader);
			tradeRandoms.Add(toTraderAmount);
			tradeRandoms.Add(fromTrader);
			tradeRandoms.Add(type);
		}
		return tradeRandoms;
	}

	private void NewTradeRandomNumbers()
	{
		List<int> tradeRandoms = GenerateNewRandomNumbers();
		for (int i = 0; i < maxTrades; i++)
		{
			for (int j = 0; j < synchronizedValues; j++)
			{
				randomTradeValues[i, j] = tradeRandoms[i* synchronizedValues+ j];
			}
			
		}
	}

	public void AcceptTrade(Trade trade, ResourceManager rm)
	{
		rm.ChangeRessourceAmount(trade.toTrader.resource, -trade.toTraderAmount);
		if (trade.type == tradeType.ship)
		{
			SpawnShip(trade,rm);
		}

		acceptedTrades++;
		if (acceptedTrades ==4)
		{
			StartCoroutine("AnnounceNewTrades", 10);
		}
	}

	private void SpawnShip(Trade trade,ResourceManager rm)
	{
		GameObject obj= Instantiate(shipPrefab, new Vector3(generator.xSize / 2, 0, generator.zSize / 2), Quaternion.identity);
		Ship ship = obj.GetComponent<Ship>();
		ship.trade = trade;
		ship.rm = rm;
		//Get Waypointcurve At child index from team. Has to be setupup correct in scene
		Transform curveTransform = wayPointsParent.transform.GetChild(rm.mainbuilding.team);
		ship.curve = curveTransform.GetComponent<BGCurve>();
		ship.math = curveTransform.GetComponent<BGCcMath>();
	}
	
	public void StartTradeOffer()
	{
		StartCoroutine("AnnounceNewTrades", 3);
	}

	private IEnumerator AnnounceNewTrades(int duration)
	{
		OnGenerateNewTrades(duration);
		yield return new WaitForSeconds(duration);

		GenerateNewTrades(maxTrades);
	}
}
public enum tradeType
{
	ship,
	foot,
	instant
}