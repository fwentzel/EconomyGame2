public class Farm : Building
{	
	public int unitsPerIntervall;


	public override void OnBuild(bool subtractResource = true)
	{
		resourceManager.AddRessource(resource.food, unitsPerIntervall);
		base.OnBuild(subtractResource);
	}

	public override void DestroyBuilding()
	{
		resourceManager.AddRessource(resource.food, -unitsPerIntervall);
		base.DestroyBuilding();
	}

	protected override void OnLevelUp()
	{
		base.OnLevelUp();
		unitsPerIntervall++;
		resourceManager.AddRessource(resource.food, 1);

	}

	protected override void TriggerBonusLevel()
	{
		unitsPerIntervall *= 2;
	}

	protected override string GetStats()
	{
		string stats = "Name: " + name + "\nTeam: " + team + "\nLevel: " + level;
		 stats  +="\nCapacity: " + unitsPerIntervall;

		return stats;
	}
}
