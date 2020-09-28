using UnityEngine;

public class Mine : Building
{
	public int unitsPerIntervall;
	private Collider[] overlapResults = new Collider[5];

	private void Awake()
	{
		PlacementController.instance.SetCanBuild(false);
	}
	public override void OnBuild(bool subtractResource = true)
	{
		resourceManager.ChangeRessourceAmount(resource.stone, unitsPerIntervall);
		base.OnBuild(subtractResource);
	}

	public override void DestroyBuilding()
	{
		resourceManager.ChangeRessourceAmount(resource.stone, -unitsPerIntervall);
		base.DestroyBuilding();
	}
	protected override void OnLevelUp()
	{
		base.OnLevelUp();
		unitsPerIntervall++;
		resourceManager.ChangeRessourceAmount(resource.stone, 1);
	}
	protected override void TriggerBonusLevel()
	{
		unitsPerIntervall *= 2;
	}
	public override string GetStats()
	{
		return $"Mine\nLevel {level}\nGenerates {unitsPerIntervall} Stone";
	}

	public override void CheckCanBuild(Collider other, bool onEnter)
	{
		if(other==null)return;
		
		if (onEnter)
		{
			if (other.tag.Equals("RockResource"))
			{
				int numFound = Physics.OverlapBoxNonAlloc(transform.position, Vector3.one * .5f, overlapResults);
				for (int i = 0; i < numFound; i++)
				{
					if (overlapResults[i].gameObject == this.gameObject) continue;
					if (overlapResults[i].tag.Equals("Placeable"))
					{
						PlacementController.instance.SetCanBuild(false);
						return;
					}
				}
				bool b=Vector3.Distance(ResourceUiManager.instance.activeResourceMan.mainbuilding.transform.position, transform.position) <= PlacementController.instance.maxPlacementRange;
				PlacementController.instance.SetCanBuild(b);
			}
		}
		else
		{
			if (other.tag.Equals("RockResource"))
			{
				PlacementController.instance.SetCanBuild(false);
			}
		}
	}
}
