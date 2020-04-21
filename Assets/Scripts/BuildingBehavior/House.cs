public class House : Building
{
	public int capacity;


	public override void OnBuild(bool subtractResource=true)
	{
		base.OnBuild(subtractResource);
		GameManager.instance.OnCalculateIntervall += UpdateContextUi;
		resourceManager.AddRessource(resource.citizens, capacity/2);
		resourceManager.mainBuilding.maxCitizens += capacity;
		
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

	public override bool LevelUp()
	{
		base.LevelUp();
		capacity++;
		resourceManager.mainBuilding.maxCitizens += 1;
		resourceManager.AddRessource(resource.citizens, 1);
		return true;
	}

}
