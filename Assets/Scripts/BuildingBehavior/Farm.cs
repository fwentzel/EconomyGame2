public class Farm : Building
{
	public int unitsPerDay;
	

	public void OnBuild()
	{
		GameManager.OnNewDay += UpdateContextUi;
		resourceManager.AddRessource(resource.food, unitsPerDay);
	}

	public override void DestroyBuilding()
	{
		GameManager.OnNewDay -= UpdateContextUi;
		resourceManager.AddRessource(resource.food, -unitsPerDay);
		base.DestroyBuilding();

	}
	public override bool LevelUp()
	{
		base.LevelUp();
		unitsPerDay++;
		resourceManager.AddRessource(resource.food, 1);
		return true;

	}
	protected override string GetStats()
	{
		string stats = "Name: " + name + "\nTeam: " + team.teamID + "\nLevel: " + level;
		 stats  +="\nCapacity: " + unitsPerDay;

		return stats;
	}
}
