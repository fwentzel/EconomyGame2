using System;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
	public static UiManager instance { get; private set; }
	public ResourceManager currentRessouceManagerToShow;
	public Transform cityResourcePanel;

	public RessourceStartvalue res;

	public GameObject buildingContextUiCanvas;
	public Text buildingContextUiText { get; private set; }
	public Button buildingContextUiLevelUpButton { get; private set; }
	public Text buildingContextUiLevelCostText { get; private set; }

	public GameObject mainBuildingContextUiCanvas;
	public Text mainBuildingContextUiText { get; private set; }
	public Text mainBuildingContextUiTaxesText { get; private set; }
	public Slider mainBuildingContextUiTaxesSlider { get; private set; }

	public GameObject menuPanel;
	public GameObject settingsPanel;

	public GameObject tradeUiCanvas;
	public GameObject newTradesTimerParent { get; private set; }
	public Text newTradesInText { get; private set; }
	public Image newTradesInImage { get; private set; }

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
	}

	private void Start()
	{
		UpdateRessourceUI();
	}

	private void SetupUiElements()
	{
		buildingContextUiText = buildingContextUiCanvas.transform.GetChild(0).Find("ContextText").GetComponent<Text>();
		buildingContextUiLevelUpButton = buildingContextUiCanvas.transform.GetChild(0).Find("LevelUpButton").GetComponent<Button>();
		buildingContextUiLevelCostText = buildingContextUiLevelUpButton.GetComponentInChildren<Text>();

		mainBuildingContextUiText = mainBuildingContextUiCanvas.transform.GetChild(0).Find("ContextText").GetComponent<Text>();
		mainBuildingContextUiTaxesText = mainBuildingContextUiCanvas.transform.GetChild(0).Find("TaxesText").GetComponent<Text>(); ;
		mainBuildingContextUiTaxesSlider = mainBuildingContextUiCanvas.transform.GetChild(0).GetComponentInChildren<Slider>();

		newTradesTimerParent = tradeUiCanvas.transform.Find("Timer").gameObject;
		newTradesInText = newTradesTimerParent.transform.Find("NewTradesTimerText").GetComponent<Text>();
		newTradesInImage = newTradesTimerParent.transform.Find("NewTradeTimerForeground").GetComponent<Image>();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			OpenMenu(tradeUiCanvas);
		}
		if (Input.GetMouseButtonDown(1))
		{
			CloseAll();
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			OpenMenu(menuPanel);
		}
	}

	public void UpdateRessourceUI()
	{
		if (currentRessouceManagerToShow == null)
			return;
		foreach (Resource res in res.startValues)
		{
			if (res.uiDisplay != null)
				res.uiDisplay .text= currentRessouceManagerToShow.GetAmountUI(res.resource);
			
		}
	}

	public void UpdateUiElement(Text text, String newText)
	{
		text.text = newText;
	}

	public void UpdateUiElement(Button button, bool interactable)
	{
		button.interactable = interactable;
	}

	public void UpdateUiElement(Slider slider, int value)
	{
		slider.value = value;
	}

	public void CloseAll()
	{
		tradeUiCanvas.SetActive(false);
		buildingContextUiCanvas.SetActive(false);
		mainBuildingContextUiCanvas.SetActive(false);
		settingsPanel.SetActive(false);
		menuPanel.SetActive(false);
	}

	public void OpenContext(GameObject canvasToOpen, Vector3 pos)
	{
		CloseAll();
		canvasToOpen.SetActive(true);
		canvasToOpen.transform.position = new Vector3(pos.x, canvasToOpen.transform.position.y, pos.z);
	}

	public void OpenMenu(GameObject menuToOpen)
	{
		bool wasActive = menuToOpen.activeSelf;
		CloseAll();
		if (wasActive == false)
			menuToOpen.SetActive(true);
	}

}