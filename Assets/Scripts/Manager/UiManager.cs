using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{


	public static UiManager instance { get; private set; }
	public ResourceManager CurrentRessouceManagerToShow { get => currentRessouceManagerToShow; set => SetRM(value); }

	[SerializeField] GameObject mainCanvas = default;
	[SerializeField] RessourceStartvalue res = default;

	ResourceManager currentRessouceManagerToShow;

	Transform settingsPanel;
	Transform traderPanel;
	Transform menuPanel;

	Transform buildingContextPanel = default;
	Text buildingContextUiText;
	Button buildingContextUiLevelUpButton;
	Text buildingContextUiLevelCostText;

	Transform mainbuildingContextPanel = default;
	Text mainbuildingContextUiText;
	Text mainbuildingContextUiTaxesText;
	Slider mainbuildingContextUiTaxesSlider;

	GameObject newTradesTimerParent;
	Text newTradesInText;
	Image newTradesInImage;

	private void Awake()
	{
		//singleton Check
		if (instance == null)
			instance = this;
		else
			Destroy(this);

		SetupUiElements();
		foreach (Resource resource in res.startValues)
		{
			resource.SearchUiDisplay();
		}
		CloseAll();
	}
	private void Start()
	{
		TradeManager.instance.OnGenerateNewTrades += (int arrivalIn) => StartCoroutine("StartNewTradeTimer", arrivalIn);
	}

	private IEnumerator StartNewTradeTimer(int arrivalIn)
	{
		float normalizedTime = arrivalIn;
		newTradesTimerParent.SetActive(true);

		while (normalizedTime >= 0)
		{
			newTradesInText.text = Mathf.CeilToInt(normalizedTime) + " seconds!";
			newTradesInImage.fillAmount = normalizedTime / arrivalIn;
			normalizedTime -= Time.deltaTime;
			yield return null;
		}
		newTradesTimerParent.SetActive(false);
	}

	void SetRM(ResourceManager rm)
	{
		if (currentRessouceManagerToShow != null)
			//Stop listening to old RM Event
			currentRessouceManagerToShow.OnResourceChange -= UpdateRessourceUI;

		//Set new Manager and start listening to new Event
		currentRessouceManagerToShow = rm;
		currentRessouceManagerToShow.OnResourceChange += UpdateRessourceUI;
	}

	private void SetupUiElements()
	{
		menuPanel= mainCanvas.transform.Find("MenuPanel");
		settingsPanel = mainCanvas.transform.Find("SettingsPanel");
		traderPanel = mainCanvas.transform.Find("TraderPanel");
		buildingContextPanel = mainCanvas.transform.Find("BuildingContextPanel");
		mainbuildingContextPanel = mainCanvas.transform.Find("MainBuildingContextPanel");

		buildingContextUiText = buildingContextPanel.transform.GetChild(0).Find("ContextText").GetComponent<Text>();
		buildingContextUiLevelUpButton = buildingContextPanel.transform.GetChild(0).Find("LevelUpButton").GetComponent<Button>();
		buildingContextUiLevelCostText = buildingContextUiLevelUpButton.GetComponentInChildren<Text>();

		mainbuildingContextUiText = mainbuildingContextPanel.transform.GetChild(0).Find("ContextText").GetComponent<Text>();
		mainbuildingContextUiTaxesText = mainbuildingContextPanel.transform.GetChild(0).Find("TaxesText").GetComponent<Text>(); ;
		mainbuildingContextUiTaxesSlider = mainbuildingContextPanel.transform.GetChild(0).GetComponentInChildren<Slider>();

		newTradesTimerParent = traderPanel.transform.Find("Timer").gameObject;
		newTradesInText = newTradesTimerParent.transform.Find("NewTradesTimerText").GetComponent<Text>();
		newTradesInImage = newTradesTimerParent.transform.Find("NewTradeTimerForeground").GetComponent<Image>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			OpenMenu(traderPanel.gameObject);
		}
		if (Input.GetMouseButtonDown(1))
		{
			CloseAll();
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			OpenMenu(menuPanel.gameObject);
		}
	}

	public void UpdateRessourceUI()
	{
		if (currentRessouceManagerToShow == null)
			return;
		foreach (Resource res in res.startValues)
		{
			res.uiDisplay.text = currentRessouceManagerToShow.GetAmountUI(res.resource);
		}
	}

	public void CloseAll()
	{
		traderPanel.gameObject.SetActive(false);
		buildingContextPanel.gameObject.SetActive(false);
		mainbuildingContextPanel.gameObject.SetActive(false);
		settingsPanel.gameObject.SetActive(false);
		menuPanel.gameObject.SetActive(false);
	}

	public void OpenMenu(GameObject menuToOpen)
	{
		if (menuToOpen == null)
		{
			OpenMenu(menuPanel.gameObject);
		}
		bool wasActive = menuToOpen.activeSelf;
		CloseAll();
		if (wasActive == false)
			menuToOpen.SetActive(true);
	}

	public void OpenContext( Building building)
	{
		CloseAll();
		GameObject canvasToOpen= building is Mainbuilding?  mainbuildingContextPanel.gameObject:buildingContextPanel.gameObject;
		canvasToOpen.SetActive(true);
		if (building is Mainbuilding)
			UpdateContextUi(building as Mainbuilding);
		else
			UpdateContextUi(building);
	}

	public void UpdateContextUi(Building building)
	{
		buildingContextUiText.text = building.GetStats();
		buildingContextUiLevelCostText.text = building.levelCost.ToString();
		buildingContextUiLevelUpButton.interactable = building.canLevelUp;
	}

	public void UpdateContextUi(Mainbuilding mainbuilding)
	{
		mainbuildingContextUiTaxesText.text = "Taxes: " + mainbuilding.Taxes + " /10 per citizen";
		mainbuildingContextUiText.text = mainbuilding.GetStats();
		mainbuildingContextUiTaxesSlider.value = mainbuilding.Taxes;
	}



}