using UnityEngine;
using System.Collections;

public class TradeVehicle : MonoBehaviour
{
	public int holdUpCost { get; private set; } = 150;
	public int holdUpDuration { get; private set; } = 5;
	internal Trade trade;
	internal ResourceManager rm;
	internal bool isStopped=false;

  public virtual void SetUp(ResourceManager resourceManager,Trade trade){
	  rm=resourceManager;
	  this.trade=trade;
  }

    protected virtual IEnumerator UnloadCoroutine(float timeBeforeUnload)
	{
		yield return new WaitForSeconds(timeBeforeUnload);
		rm.ChangeRessourceAmount(trade.fromTrader.resource, trade.fromTraderAmount);
		if (SelectionManager.instance.selectedObject = gameObject)
		{
			SelectionManager.instance.Deselect();
		}
		Destroy(gameObject);
	}

	public virtual IEnumerator HoldUpCoroutine( ResourceManager rm=null)
	{
		if (rm == null)
			//AI will pass its RM, so only player will have value 0
			rm = ResourceUiManager.instance.activeResourceMan;

		rm.ChangeRessourceAmount(resource.gold, -holdUpCost);
		isStopped = true;
		yield return new WaitForSeconds(holdUpDuration);
		isStopped = false;
	}

	private void OnMouseUp()
	{
		SelectionManager.instance.selectedObject = gameObject;
		ContextUiManager.instance.OpenContext(this);
	}

}
