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
        return $"House\nLevel  {level} \nCapacity: {currentAmount} / {capacity}";
    }
    protected override void TriggerBonusLevel()
    {
        capacity *= 2;
        ChangeCitizenAmount(2);
    }

    protected override void OnLevelUp()
    {
        base.OnLevelUp();
        capacity+=2;
        resourceManager.mainbuilding.maxCitizens += 2;
		ResourceUiManager.instance.UpdateRessourceUI(resource.citizens);
        
    }

}
