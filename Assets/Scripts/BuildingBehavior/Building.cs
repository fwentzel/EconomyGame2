﻿using UnityEngine;
using UnityEngine.EventSystems;


public class Building : MonoBehaviour
{
	public ResourceManager resourceManager;
	public Team team;
	public Mesh[] meshlevels;
	public int buildCost=50;
	public int levelCost = 100;

	public int level { get; private set; } = 1;
	
	int maxLevel = 7;

	protected virtual void OnMouseOver()
	{
		if (Input.GetMouseButtonDown(0) &&
			GameManager.instance.team == team &&
			!EventSystem.current.IsPointerOverGameObject())//so no other Building behind floating UI gets triggered
		{
			SelectionManager.instance.selectedObject = this.gameObject;
			UiManager.instance.OpenContext(UiManager.instance.buildingContextUiCanvas, transform.position);
			UpdateContextUi();
		}
	}

	public virtual bool LevelUp()
	{
		if (level == maxLevel)
			return false;
		level++;
		resourceManager.AddRessource(resource.money, -levelCost);
		VFXManager.instance.PlayEffect(VFXManager.instance.levelUpEffect, transform.position);
		SetLevelMesh();
		return true;
	}

	public void SetLevelMesh()
	{
		if (meshlevels.Length > level - 1) {
			GetComponent<MeshFilter>().sharedMesh = meshlevels[level - 1];
			GetComponent<BoxCollider>().size = meshlevels[level - 1].bounds.extents * 2;
			GetComponent<BoxCollider>().center = meshlevels[level - 1].bounds.center;
		}
	}

	public virtual void DestroyBuilding()
	{
		resourceManager.mainBuilding.buildings.Remove(this);
		Destroy(this.gameObject);
	}

	public virtual void UpdateContextUi()
	{
		UiManager.instance.UpdateUiElement(UiManager.instance.buildingContextUiText, GetStats());
		UiManager.instance.UpdateUiElement(UiManager.instance.buildingContextUiLevelCostText, levelCost.ToString());
		UiManager.instance.UpdateUiElement(UiManager.instance.buildingContextUiLevelUpButton,
											resourceManager.GetAmount(resource.money) >= levelCost && level < maxLevel);
	}


	protected virtual string GetStats()
	{
		string stats = "Name: " + name+ "\nTeam: " + team.teamID + "\nLevel: " + level;
		return stats;
	}

}