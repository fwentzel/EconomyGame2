public class House : Building
{
    public int capacity;
    public int currentAmount {get;private set;}= 0;

    public override void OnBuild(bool subtractResource = true)
    {
        ChangeCitizenAmount(capacity);
        resourceManager.mainbuilding.maxCitizens += capacity;
        base.OnBuild(subtractResource);
    }

    public void ChangeCitizenAmount(int amount)
    {
        currentAmount += amount;
        resourceManager.ChangeRessourceAmount(resource.citizens, amount);
    }

    public override void DestroyBuilding()
    {
        
		ChangeCitizenAmount(-currentAmount);
        resourceManager.mainbuilding.maxCitizens -= capacity;
        base.DestroyBuilding();
    }
    public override string GetStats()
    {
        return "Type: House" + "\nTeam: " + team + "\nLevel: " + level + "\nCurrent: " + currentAmount + "\nCapacity: " + capacity;
    }
    protected override void TriggerBonusLevel()
    {
        capacity += 2;
        ChangeCitizenAmount(1);
    }

    protected override void OnLevelUp()
    {
        base.OnLevelUp();
        capacity++;
        resourceManager.mainbuilding.maxCitizens += 1;
		ResourceUiManager.instance.UpdateRessourceUI(resource.citizens);
        
    }

}
