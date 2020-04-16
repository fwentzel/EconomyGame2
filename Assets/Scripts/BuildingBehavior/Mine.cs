using UnityEngine;

public class Mine : Building
{
	public int unitsPerIntervall;
	private Collider[] overlapResults = new Collider[5];

	private void Awake()
	{
		PlacementController.instance.SetCanBuild(false);
	}
	public override void OnBuild()
	{
		GameManager.instance.OnCalculateIntervall += UpdateContextUi;
		resourceManager.AddRessource(resource.stone, unitsPerIntervall);
	}

	public override void DestroyBuilding()
	{
		GameManager.instance.OnCalculateIntervall -= UpdateContextUi;
		resourceManager.AddRessource(resource.stone, -unitsPerIntervall);
		base.DestroyBuilding();

	}
	public override bool LevelUp()
	{
		base.LevelUp();
		unitsPerIntervall++;
		resourceManager.AddRessource(resource.stone, 1);
		return true;

	}
	protected override void TriggerBonusLevel()
	{
		unitsPerIntervall *= 2;
	}
	protected override string GetStats()
	{
		string stats = "Name: " + name + "\nTeam: " + team.teamID + "\nLevel: " + level;
		stats += "\nCapacity: " + unitsPerIntervall;

		return stats;
	}

	public override void CheckCanBuild(Collider other, bool onEnter)
	{
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

				PlacementController.instance.SetCanBuild(true);
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
