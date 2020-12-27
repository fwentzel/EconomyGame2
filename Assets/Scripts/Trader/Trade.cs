using System;

[System.Serializable]
public struct Trade 
{
	public tradeType type;
	public Resource fromTrader;
	public Resource toTrader;
	public int fromTraderAmount;
	public int toTraderAmount;

    public override string ToString()
    {	
        return string.Format($"{fromTraderAmount} {fromTrader.resource} via {type} for {toTraderAmount} {toTrader.resource}");
    }
}
