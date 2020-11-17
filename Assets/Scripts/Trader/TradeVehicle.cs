using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class TradeVehicle : MonoBehaviour,ISelectable
{
	public int holdUpCost { get; private set; } = 500;
	public int holdUpDuration { get; private set; } = 5;
	Trade trade;
	protected ResourceManager rm;
	protected NavMeshAgent agent;

  public virtual void SetUp(ResourceManager resourceManager,Trade trade){
	  rm=resourceManager;
	  this.trade=trade;
  }

    protected virtual IEnumerator UnloadCoroutine(float timeBeforeUnload)
	{
		yield return new WaitForSeconds(timeBeforeUnload);
		rm.ChangeRessourceAmount(trade.fromTrader.resource, trade.fromTraderAmount);
		if (SelectionManager.instance?.selectedObject == gameObject)
		{
			SelectionManager.instance.Deselect();
		}
		TradeManager.instance.tradeVehicles.Remove(this);
		Destroy(gameObject);
	}

	public virtual IEnumerator HoldUpCoroutine( ResourceManager rm=null)
	{
		if (rm == null)
			//AI will pass its RM, so only player will have value 0
			rm = ResourceUiManager.instance.activeResourceMan;

		rm.ChangeRessourceAmount(resource.gold, -holdUpCost);

		agent.isStopped = true;
		yield return new WaitForSeconds(holdUpDuration);
		agent.isStopped  = false;
	}
	public void HoldUp(){
		StartCoroutine(HoldUpCoroutine());
	}

    public bool IsSelectable()
    {
        return rm.mainbuilding.team != GameManager.instance.localPlayer.team;
    }
}
