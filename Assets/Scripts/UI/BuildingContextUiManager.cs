using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BuildingContextUiManager : MonoBehaviour
{
	public static BuildingContextUiManager instance { get; private set; }
	
	Transform buildingContextPanel = null;
	Text buildingContextUiText;
	Button buildingContextUiLevelUpButton;
	Text buildingContextUiLevelCostText;

	Transform mainbuildingContextPanel = null;
	Text mainbuildingContextUiText;
	Text mainbuildingContextUiTaxesText;
	Slider mainbuildingContextUiTaxesSlider;

	// Use this for initialization
	private void Awake()
	{
		//singleton Check
		if (instance == null)
			instance = this;
		else
			Destroy(this);
		SetupUiElements();
	}

	private void Start()
	{
		GameManager.instance.OnCalculateIntervall += TriggerUpdateContextFromCalcIntervall;
	}

	void TriggerUpdateContextFromCalcIntervall()
	{
		GameObject selected = SelectionManager.instance.selectedObject;
		if (selected != null)
			UpdateContextUi(selected.GetComponent<Building>());
	}

	private void SetupUiElements()
	{
		buildingContextPanel = transform.Find("BuildingContextPanel");
		mainbuildingContextPanel = transform.Find("MainBuildingContextPanel");

		buildingContextUiText = buildingContextPanel.transform.GetChild(0).Find("ContextText").GetComponent<Text>();
		buildingContextUiLevelUpButton = buildingContextPanel.transform.GetChild(0).Find("LevelUpButton").GetComponent<Button>();
		buildingContextUiLevelCostText = buildingContextUiLevelUpButton.GetComponentInChildren<Text>();

		mainbuildingContextUiText = mainbuildingContextPanel.transform.GetChild(0).Find("ContextText").GetComponent<Text>();
		mainbuildingContextUiTaxesText = mainbuildingContextPanel.transform.GetChild(0).Find("TaxesText").GetComponent<Text>(); ;
		mainbuildingContextUiTaxesSlider = mainbuildingContextPanel.transform.GetChild(0).GetComponentInChildren<Slider>();

	}

	public void OpenContext(Building building)
	{
		if(building is Mainbuilding)
		{
			mainbuildingContextPanel.gameObject.SetActive(true);
			buildingContextPanel.gameObject.SetActive(false);
			UpdateContextUi(building as Mainbuilding);
		}
		else
		{
			mainbuildingContextPanel.gameObject.SetActive(false);
			buildingContextPanel.gameObject.SetActive(true);
			building.CheckCanLevelUp();
			UpdateContextUi(building);
		}
	}

	public void UpdateContextUi(Building building)
	{
		buildingContextUiText.text = building.GetStats();
		buildingContextUiLevelCostText.text = building.levelCost.ToString();
		buildingContextUiLevelUpButton.interactable = building.CheckCanLevelUp();
	}

	public void UpdateContextUi(Mainbuilding mainbuilding)
	{
		mainbuildingContextUiTaxesText.text = "Taxes: " + mainbuilding.Taxes + " /10 per citizen";
		mainbuildingContextUiText.text = mainbuilding.GetStats();
		mainbuildingContextUiTaxesSlider.value = mainbuilding.Taxes;
	}

	public void CloseContextMenus()
	{
			mainbuildingContextPanel.gameObject.SetActive(false);
			buildingContextPanel.gameObject.SetActive(false);
	}
}
