public class House : Building
{
	public int capacity;


	public override void OnBuild(bool subtractResource=true)
	{
		resourceManager.AddRessource(resource.citizens, capacity/2);
		resourceManager.mainBuilding.maxCitizens += capacity;
		base.OnBuild(subtractResource);
	}

	public override void DestroyBuilding()
	{
		resourceManager.AddRessource(resource.citizens, -capacity/2);
		resourceManager.mainBuilding.maxCitizens -= capacity;
		base.DestroyBuilding();
	}
	protected override string GetStats()
	{
		string stats = "Name: " + name + "\nTeam: " + team + "\nLevel: " + level;
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
		resourceManager.mainBuilding.maxCitizens += 1;
		resourceManager.AddRessource(resource.citizens, 1);
	}

}
