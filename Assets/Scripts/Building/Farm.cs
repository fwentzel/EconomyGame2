public class Farm : Building
{	
	public int unitsPerIntervall;


	public override void OnBuild(bool subtractResource = true)
	{
		resourceManager.ChangeRessourceAmount(resource.food, unitsPerIntervall);
		base.OnBuild(subtractResource);
	}

	public override void DestroyBuilding()
	{
		resourceManager.ChangeRessourceAmount(resource.food, -unitsPerIntervall);
		base.DestroyBuilding();
	}

	protected override void OnLevelUp()
	{
		base.OnLevelUp();
		unitsPerIntervall++;
		resourceManager.ChangeRessourceAmount(resource.food, 1);

	}

	protected override void TriggerBonusLevel()
	{
		unitsPerIntervall *= 2;
	}

	public override string GetStats()
	{
		string stats = "Type: Farm" + "\nTeam: " + team + "\nLevel: " + level;
		 stats  +="\nFood/Intervall: " + unitsPerIntervall;

		return stats;
	}
}
