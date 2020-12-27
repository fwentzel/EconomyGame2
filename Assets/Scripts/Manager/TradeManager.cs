using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Linq;


public class TradeManager : MonoBehaviour
{

    public event Action<int> OnGenerateNewTrades = delegate { };

    [SerializeField] Resource[] tradingResources = null;
    [SerializeField] GameObject tradeUiPanel = null;
    public TradeTypeToPrefab[] tradeTypePrefabMap = default;
    public static TradeManager instance { get; private set; }
    public Dictionary<Trade, TradeElement> tradeToElementMapping { get; private set; }
    public int tradeCooldown { get; private set; } = 3;
    public TradeElement[] tradeElements { get; private set; } = new TradeElement[6];



    public List<TradeVehicle> tradeVehicles { get; private set; } = new List<TradeVehicle>();


    MapGenerator generator;
    int acceptedTrades = 0;
    int maxTrades = 6;
    int synchronizedValues = 4;//Workaround to know how big the stepsize for listtransformation is
    int[,] randomTradeValues;

    [SerializeField] TradeConstraint[] constraints;



    private void Awake()
    {
        //singleton Check
        if (instance == null)
            instance = this;
        else
            Destroy(this);
        generator = FindObjectOfType<MapGenerator>();
        tradeToElementMapping = new Dictionary<Trade, TradeElement>();
        randomTradeValues = new int[maxTrades, synchronizedValues];
    }

    private void Start()
    {
        GameManager.instance.OnGameStart += Setup;
        GameManager.instance.OnGameStart += () => StartCoroutine(AnnounceNewTradesCoroutine(3));
    }

    void Setup()
    {
        int i = 0;
        //TODO ABhängigkeit weg
        foreach (TradeElement tradeElement in tradeUiPanel.transform.GetComponentsInChildren<TradeElement>())
        {
            tradeElements[i] = tradeElement;
            tradeElements[i].gameObject.SetActive(false);
            i++;
        }
    }


    private void GenerateNewTrades(int amount)
    {
        tradeToElementMapping.Clear();
        NewTradeRandomNumbers();
        for (int i = 0; i < amount; i++)
        {
            Trade newTrade = GenerateTrade(i);

            tradeToElementMapping[newTrade] = tradeElements[i];
            tradeElements[i].Init(newTrade, ResourceUiManager.instance?.activeResourceMan);
            tradeElements[i].gameObject.SetActive(true);
        }
        acceptedTrades = 0;
    }

    private Trade GenerateTrade(int i)
    {
        bool useConstraint = i < constraints.Length;
        // [0,4] , [50,100] , [0,4] 
        Trade newTrade = new Trade
        {

            toTrader = useConstraint && constraints[i].toTrader != resource.none ? Array.Find(tradingResources, x => x.resource == constraints[i].toTrader) : tradingResources[randomTradeValues[i, 0]],
            toTraderAmount = useConstraint && constraints[i].toTraderAmount > -1 ? constraints[i].toTraderAmount : randomTradeValues[i, 1],

            fromTrader = useConstraint && constraints[i].fromTrader != resource.none ? Array.Find(tradingResources, x => x.resource == constraints[i].fromTrader) : tradingResources[randomTradeValues[i, 2]],
            fromTraderAmount = useConstraint && constraints[i].fromTraderAmount > -1 ? constraints[i].fromTraderAmount : randomTradeValues[i, 1] - 30,
            // type = tradeType.ship
            type = (tradeType)Enum.GetValues(typeof(tradeType)).GetValue(randomTradeValues[i, 3])

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
            int fromTrader = toTrader;
            
            //To get different Resources for in an output
            while (fromTrader == toTrader)
            {
                fromTrader = Random.Range(0, tradingResources.Length);
            }

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

    public void AcceptTrade(Trade trade, ResourceManager rm, bool isDebug = false)
    {
        rm.ChangeRessourceAmount(trade.toTrader.resource, -trade.toTraderAmount);
        SpawnVehicle(trade, rm);

        if (isDebug) return;

        tradeToElementMapping[trade].DisableElement();

        acceptedTrades++;
        if (acceptedTrades == 4)
        {
            StartCoroutine(AnnounceNewTradesCoroutine(10));
        }
    }

    private void SpawnVehicle(Trade trade, ResourceManager rm)
    {
        GameObject prefab = null;
        foreach (var item in tradeTypePrefabMap)
        {
            if (item.tradeType == trade.type)
                prefab = item.prefab;
        }
        if (prefab == null)
        {
            Debug.LogWarning("Unknown Tradetype for tradetype to Pefab mapping!");
            return;
        }
        GameObject obj = Instantiate(prefab, new Vector3(0, 40, 0), Quaternion.identity);
        TradeVehicle vehicle = obj.GetComponent<TradeVehicle>();
        vehicle.SetUp(rm, trade);
        tradeVehicles.Add(vehicle);

    }


    private IEnumerator AnnounceNewTradesCoroutine(int duration)
    {
        MessageSystem.instance?.Message("The Trader is about to offer something else!");
        OnGenerateNewTrades(duration);
        yield return new WaitForSeconds(duration);

        GenerateNewTrades(maxTrades);
    }

    public Resource GetTradingResource(resource res)
    {
        for (int i = 0; i < tradingResources.Length; i++)
        {
            if (tradingResources[i].resource == res)
                return tradingResources[i];
        }
        return null;
    }
}
public enum tradeType
{
    ship,
    foot,
    instant,

}

[System.Serializable]
class TradeConstraint
{

    public resource toTrader = resource.none;
    public int toTraderAmount = -1;
    public resource fromTrader = resource.none;
    public int fromTraderAmount = -1;


}
