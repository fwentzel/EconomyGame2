using UnityEngine;



public class Building : MonoBehaviour
{

	public ResourceManager resourceManager;
	public int team = -1;
	public Sprite sprite;

	[SerializeField] Mesh[] meshlevels=null;

	public int buildCost { get; private set; } = 50;
	public int levelCost { get; private set; } = 100;
	public int level { get; private set; } = 1;
	public bool canLevelUp { get; private set; } = false;

	int triggerBonuslevelAt = 4;
	int maxLevel = 7;

	protected virtual void OnMouseEnter()
	{
		if (GameManager.instance.localPlayer.team == team )
		{
			SelectionManager.instance.hoveredObject = gameObject;
		}

	}

	protected virtual void OnMouseExit()
	{
		SelectionManager.instance.hoveredObject = null;
	}

	public bool LevelUp()
	{
		if (level == maxLevel)
			return false;
		level++;
		OnLevelUp();
		return true;
	}

	protected virtual void OnLevelUp()
	{
		if (level == triggerBonuslevelAt || level == maxLevel)
			TriggerBonusLevel();

		resourceManager.ChangeRessourceAmount(resource.money, -levelCost);
		VFXManager.instance.PlayEffect(VFXManager.instance.levelUpEffect, transform.position);
		SetLevelMesh();
		levelCost = (int)(levelCost * 1.5f);
		CheckCanLevelUp();
		
	}

	protected virtual void TriggerBonusLevel() { }

	public void SetLevelMesh()
	{
		if (meshlevels.Length > level - 1)
		{
			GetComponent<MeshFilter>().sharedMesh = meshlevels[level - 1];
			GetComponent<BoxCollider>().size = meshlevels[level - 1].bounds.extents * 2;
			GetComponent<BoxCollider>().center = meshlevels[level - 1].bounds.center;
		}
	}

	public virtual void OnBuild(bool subtractResource = true)
	{
		if (subtractResource)
			resourceManager.ChangeRessourceAmount(resource.money, -buildCost);
	}

	public virtual void DestroyBuilding()
	{
		resourceManager.ChangeRessourceAmount(resource.money, (int)(buildCost * .6f));
		resourceManager.mainbuilding.buildings.Remove(this);
		Destroy(this.gameObject);
	}


	public virtual string GetStats()
	{
		return "Type: Building" + "\nTeam: " + team + "\nLevel: " + level;
	}

	public virtual void CheckCanBuild(Collider other, bool onEnter)
	{
		if (other.tag.Equals("Ground"))
			return;
		if (onEnter)
		{
			PlacementController.instance.SetCanBuild(false);
		}
		else
		{
			PlacementController.instance.SetCanBuild(true);
		}
	}

	public bool CheckCanLevelUp()
	{
		return resourceManager.GetAmount(resource.money) >= levelCost && level < maxLevel;
	}

}
