[System.Serializable]
public struct Trade 
{
	public tradeType type;
	public Resource fromTrader;
	public Resource toTrader;
	public int fromTraderAmount;
	public int toTraderAmount;
}
