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
	public Text buildingContextUiText;
	public Button buildingContextUiLevelUpButton;
	public Text buildingContextUiLevelCostText;

	public GameObject mainBuildingContextUiCanvas;
	public Text mainBuildingContextUiText;
	public Text mainBuildingContextUiTaxesText;
	public Slider mainBuildingContextUiTaxesSlider;

	public GameObject menuPanel;
	public GameObject settingsPanel;

	public GameObject tradeUiCanvas;
	public GameObject newTradesTimerParent;
	public Text newTradesInText;
	public Image newTradesInImage;

	private void Awake()
	{
		//singleton Check
		if (instance == null)
			instance = this;
		else
			Destroy(this);

		foreach (Resource resource in res.startValues)
		{
			resource.SearchUiDisplay();
		}
	}
	private void Start()
	{
		UpdateRessourceUI();
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
		foreach (Resource res in res.startValues)
		{
			int amount = currentRessouceManagerToShow.GetAmount(res.resource);
			if(res.uiDisplay!=null)
				res.uiDisplay.text = amount.ToString();
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

	public void OpenContext(GameObject canvasToOpen,Vector3 pos)
	{
		CloseAll();
		canvasToOpen.SetActive(true);
		canvasToOpen.transform.position= new Vector3(pos.x,canvasToOpen.transform.position.y, pos.z);
	}
	
	public void OpenMenu(GameObject menuToOpen)
	{
		bool wasActive = menuToOpen.activeSelf;
		CloseAll();
		if(wasActive==false)
			menuToOpen.SetActive(true);
	}

}