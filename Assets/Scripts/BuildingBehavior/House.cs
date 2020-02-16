public class House : Building
{
	public int capacity;


	public override void OnBuild()
	{
		GameManager.OnCalculateIntervall += UpdateContextUi;
		resourceManager.AddRessource(resource.citizens, capacity);
	}

	public override void DestroyBuilding()
	{
		GameManager.OnCalculateIntervall -= UpdateContextUi;
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
