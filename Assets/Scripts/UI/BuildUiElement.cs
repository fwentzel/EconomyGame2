using UnityEngine;
using UnityEngine.UI;

public class BuildUiElement : MonoBehaviour
{
	[SerializeField] Image sprite = null;
	[SerializeField] Text buildCostText = null;
	[SerializeField] Button button = null;

	int buildCost;
	GameObject prefab;

	public void Init(GameObject buildingPrefab)
	{
		Building building = buildingPrefab.GetComponent<Building>();
		sprite.sprite = building.sprite;
		buildCost = building.buildCost;
		buildCostText.text = buildCost.ToString();

		prefab = buildingPrefab;
		button.onClick.AddListener(delegate () { InstantiateNewBuilding(); });
		ResourceUiManager.instance.activeResourceMan.OnResourceChange += CheckInteractable;
	}

	private void CheckInteractable()
	{
		bool interactable = ResourceUiManager.instance.activeResourceMan.GetAmount(resource.money) > buildCost;
		button.interactable = interactable;
	}

	void InstantiateNewBuilding()
	{
		PlacementController.instance.NewPlaceableObject(prefab);
	}


}


