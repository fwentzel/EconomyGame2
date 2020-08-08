using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class TradeManager : MonoBehaviour
{
    
	public event Action<int> OnGenerateNewTrades = delegate { };

	public static TradeManager instance { get; private set; }
	public Dictionary<Trade, TradeElement> tradeToElementMapping { get; private set; }
	public Dictionary<ResourceManager, float> tradeCooldowns { get; private set; }

	[SerializeField] Resource[] tradingResources = null;
	[SerializeField] GameObject tradeUiPanel = null;
    [SerializeField] GameObject newTradeTimerObject = null;
	[SerializeField] GameObject tradeElementPrefab = null;
	[SerializeField] GameObject wayPointsParent = null;
	[SerializeField] GameObject shipPrefab=null;

	TradeElement[] tradeElements = new TradeElement[6];
	MapGenerator generator;
	int acceptedTrades = 0;
    int maxTrades = 6;
    int synchronizedValues = 4;//Workaround to know how big the stepsize for listtransformation is
    int[,] randomTradeValues;
	int tradeCooldown=20;


	private void Awake()
    {
        //singleton Check
        if (instance == null)
            instance = this;
        else
            Destroy(this);
        generator = FindObjectOfType<MapGenerator>();
        tradeToElementMapping = new Dictionary<Trade, TradeElement>();
		tradeCooldowns = new Dictionary<ResourceManager, float>();
		randomTradeValues = new int[maxTrades, synchronizedValues];
    }

    private void Start()
	{
		foreach (ResourceManager resourceManger in CityResourceLookup.instance.resourceManagers)
		{
			tradeCooldowns.Add(resourceManger, Time.time);
		}
		int i = 0;
		foreach (TradeElement tradeElement in tradeUiPanel.transform.GetComponentsInChildren<TradeElement>())
		{
			tradeElements[i] = tradeElement;
			tradeElements[i].gameObject.SetActive(false);
			i++;
		}
	}

	public void StartTradeOffer()
	{
		StartCoroutine("AnnounceNewTrades", 3);
	}

	private void GenerateNewTrades(int amount)
    {
		tradeToElementMapping.Clear();
		NewTradeRandomNumbers();
        for (int i = 0; i < amount; i++)
        {
			//TradeElement newTradeElement = Instantiate(tradeElementPrefab, tradeUiPanel.transform).GetComponent<TradeElement>();
			Trade newTrade = GenerateTrade(i);

            tradeToElementMapping[newTrade] = tradeElements[i];
			tradeElements[i].Init(newTrade);
			tradeElements[i].gameObject.SetActive(true);
		}
        acceptedTrades = 0;
    }

    private Trade GenerateTrade(int i)
    {
		// [0,4] , [50,100] , [0,4] 
		Trade newTrade = new Trade
		{
			toTrader = tradingResources[randomTradeValues[i, 0]],
			toTraderAmount = randomTradeValues[i, 1],

			fromTrader = tradingResources[randomTradeValues[i, 2]],
			fromTraderAmount = randomTradeValues[i, 1] - 30,
			type = tradeType.ship
            // type = (tradeType)Enum.GetValues(typeof(tradeType)).GetValue(randomTradeValues[i, 3])
			
        };

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
                randomTradeValues[i, j] = tradeRandoms[i * synchronizedValues + j];
            }
        }
    }

    public void AcceptTrade(Trade trade, ResourceManager rm)
    {
        tradeToElementMapping[trade].DisableElement();
        rm.ChangeRessourceAmount(trade.toTrader.resource, -trade.toTraderAmount);
        if (trade.type == tradeType.ship)
        {
            SpawnShip(trade, rm);
        }

		tradeCooldowns[rm] = Time.time + tradeCooldown;

		acceptedTrades++;
        if (acceptedTrades == 4)
        {
            StartCoroutine("AnnounceNewTrades", 10);
        }
    }

    private void SpawnShip(Trade trade, ResourceManager rm)
    {
        GameObject obj = Instantiate(shipPrefab, new Vector3(generator.xSize / 2, 0, generator.zSize / 2), Quaternion.identity);
        Ship ship = obj.GetComponent<Ship>();
        ship.trade = trade;
        ship.rm = rm;
        //Get Waypointcurve At child index from team. Has to be setupup correct in scene
        Transform curveTransform = wayPointsParent.transform.GetChild(rm.mainbuilding.team);
        ship.curve = curveTransform.GetComponent<BGCurve>();
        ship.math = curveTransform.GetComponent<BGCcMath>();
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
