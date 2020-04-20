﻿public class Farm : Building
{
	public int unitsPerIntervall;


	public override void OnBuild()
	{
		GameManager.instance.OnCalculateIntervall += UpdateContextUi;
		resourceManager.AddRessource(resource.food, unitsPerIntervall);
	}

	public override void DestroyBuilding()
	{
		GameManager.instance.OnCalculateIntervall -= UpdateContextUi;
		resourceManager.AddRessource(resource.food, -unitsPerIntervall);
		base.DestroyBuilding();

	}
	public override bool LevelUp()
	{
		base.LevelUp();
		unitsPerIntervall++;
		resourceManager.AddRessource(resource.food, 1);
		return true;

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
