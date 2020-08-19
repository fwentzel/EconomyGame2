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
		string msg= string.Format("{0} {1} per  {2} for {3} {4}",fromTraderAmount,fromTrader.resource,type,toTraderAmount,toTrader.resource);
        return msg;
    }
}
