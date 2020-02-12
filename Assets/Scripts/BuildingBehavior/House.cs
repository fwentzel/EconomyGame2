public class House : Building, IActionOnBuild
{
	public int capacity;


	public void OnBuild()
	{
		GameManager.OnNewDay += UpdateContextUi;
		resourceManager.AddRessource(resource.citizens, capacity);
	}

	public override void DestroyBuilding()
	{
		GameManager.OnNewDay -= UpdateContextUi;
		resourceManager.AddRessource(resource.citizens, -capacity);
		base.DestroyBuilding();

	}
	protected override string GetStats()
	{
		string stats = "Name: " + name + "\nTeam: " + team.teamID + "\nLevel: " + level;
		stats += "\nCapacity: " + capacity;

		return stats;
	}
	protected override void TriggerBonusLevel()
	{
		capacity *= 2;
	}

	public override bool LevelUp()
	{
		base.LevelUp();
		capacity++;
		resourceManager.AddRessource(resource.citizens, 1);
		return true;
	}

}
