using System;
using UnityEngine;
using UnityEngine.EventSystems;


public class Building : MonoBehaviour
{
	public ResourceManager resourceManager;
	public int team = -1;
	public Mesh[] meshlevels;
	public int buildCost = 50;
	public int levelCost = 100;
	public int level { get; private set; } = 1;
	public bool canLevelUp { get; private set; } = false;
	int triggerBonuslevelAt = 4;
	int maxLevel = 7;

	protected virtual void OnMouseOver()
	{

		if (Input.GetMouseButtonDown(0))
		{
			if (PlacementController.instance.isPlacing == false &&//so no other Building gets triggered when trying to place on occupied spot
			GameManager.instance.localPlayer.team == team &&
			!EventSystem.current.IsPointerOverGameObject())//so no other Building behind floating UI gets triggered
			{
				CheckCanLevelUp();
				SelectionManager.instance.SelectedObject = gameObject;
			}
		}

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
		GameManager.instance.OnCalculateIntervall += CheckCanLevelUp;
		if (subtractResource)
			resourceManager.ChangeRessourceAmount(resource.money, -buildCost);
	}

	public virtual void DestroyBuilding()
	{
		GameManager.instance.OnCalculateIntervall -= CheckCanLevelUp;
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

	public void CheckCanLevelUp()
	{
		canLevelUp = resourceManager.GetAmount(resource.money) >= levelCost && level < maxLevel;
	}

}
