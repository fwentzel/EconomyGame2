using UnityEngine;
using UnityEngine.UI;

public class BuildUiElement : MonoBehaviour
{
	[SerializeField] Text buildCostText = null;
	[SerializeField] Button button = null;
	 RawImage rawImage = null;

	int buildCost;
	GameObject prefab;


private void Awake() {
	rawImage=GetComponent<RawImage>();
}
	public void Init(GameObject buildingPrefab)
	{
		Building building = buildingPrefab.GetComponent<Building>();
		rawImage.texture = building.renderTexture;
		buildCost = building.buildCost;
		buildCostText.text = buildCost.ToString();

		prefab = buildingPrefab;
		button.onClick.AddListener(delegate () { InstantiateNewBuilding(); });
		ResourceUiManager.instance.activeResourceMan.OnResourceChange += CheckInteractable;
	}

	private void CheckInteractable()
	{
		bool interactable = ResourceUiManager.instance.activeResourceMan.GetAmount(resource.gold) > buildCost;
		button.interactable = interactable;
	}

	void InstantiateNewBuilding()
	{
		PlacementController.instance.NewPlaceableObject(prefab);
	}


}


