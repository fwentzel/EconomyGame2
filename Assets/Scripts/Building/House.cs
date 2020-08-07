public class House : Building
{
	public int capacity;

	public override void OnBuild(bool subtractResource=true)
	{
		resourceManager.ChangeRessourceAmount(resource.citizens, capacity/2);
		resourceManager.mainbuilding.maxCitizens += capacity;
		base.OnBuild(subtractResource);
	}

	public override void DestroyBuilding()
	{
		resourceManager.ChangeRessourceAmount(resource.citizens, -capacity/2);
		resourceManager.mainbuilding.maxCitizens -= capacity;
		base.DestroyBuilding();
	}
	public override string GetStats()
	{
		string stats = "Type: House" + "\nTeam: " + team + "\nLevel: " + level;
		stats += "\nCapacity: " + capacity;

		return stats;
	}
	protected override void TriggerBonusLevel()
	{
		capacity *= 2;
	}

	protected override void OnLevelUp()
	{
		base.OnLevelUp();
		capacity++;
		resourceManager.mainbuilding.maxCitizens += 1;
		resourceManager.ChangeRessourceAmount(resource.citizens, 1);
	}

}
