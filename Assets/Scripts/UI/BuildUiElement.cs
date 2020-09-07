using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildUiElement : MonoBehaviour
{
    TMP_Text buildCostText;
    Button button => GetComponent<Button>();
    RawImage rawImage = null;

    int buildCost;
    GameObject prefab;


    private void Awake()
    {
        rawImage = transform.GetChild(1).GetComponent<RawImage>();
        buildCostText = transform.GetChild(0).GetComponent<TMP_Text>();
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


