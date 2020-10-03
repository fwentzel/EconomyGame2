using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ContextUiManager : MonoBehaviour
{
    public static ContextUiManager instance { get; private set; }

    [SerializeField] float offsetY = 3.5f;
    Transform buildingContextPanel = null;
    TMP_Text buildingContextUiText;
    Button buildingContextUiLevelUpButton;
    TMP_Text buildingContextUiLevelCostText;

    Transform mainbuildingContextPanel = null;
    TMP_Text mainbuildingContextUiText;
    TMP_Text mainbuildingContextUiTaxesText;
    Slider mainbuildingContextUiTaxesSlider;
    TMP_Text mainBuildingContextUiFoodText;

    Transform tradeVehicleContextPanel = null;
    Button tradeVehicleStopButton;
    TMP_Text tradeVechicleStopCostText;

    Canvas canvas;

    // Use this for initialization
    private void Awake()
    {
        //singleton Check
        if (instance == null)
            instance = this;
        else
            Destroy(this);
        SetupUiElements();
        CloseAll();
    }

    private void Start()
    {
        GameManager.instance.OnGameStart += () => ResourceUiManager.instance.activeResourceMan.OnResourceChange += TriggerUpdateContext;
    }

    void TriggerUpdateContext()
    {
        GameObject selected = SelectionManager.instance.selectedObject;
        if (selected == null) return;

        Building building = selected.GetComponent<Building>();
        TradeVehicle vehicle = selected.GetComponent<TradeVehicle>();
        if (building != null)
            UpdateContextUi(building);
        else if (vehicle != null)
            UpdateContextUi(vehicle);
    }

    private void SetupUiElements()
    {
        canvas = GetComponent<Canvas>();

        buildingContextPanel = transform.Find("BuildingContextPanel");
        mainbuildingContextPanel = transform.Find("MainBuildingContextPanel");
        tradeVehicleContextPanel = transform.Find("TraderVehicleContextPanel");

        buildingContextUiText = buildingContextPanel.Find("ContextText").GetComponent<TMP_Text>();
        buildingContextUiLevelUpButton = buildingContextPanel.Find("LevelUpButton").GetComponent<Button>();
        buildingContextUiLevelCostText = buildingContextUiLevelUpButton.GetComponentInChildren<TMP_Text>();

        mainbuildingContextUiText = mainbuildingContextPanel.Find("ContextText").GetComponent<TMP_Text>();
        mainbuildingContextUiTaxesText = mainbuildingContextPanel.Find("TaxesText").GetComponent<TMP_Text>();
        mainbuildingContextUiTaxesSlider = mainbuildingContextPanel.GetComponentInChildren<Slider>();
        mainBuildingContextUiFoodText = mainbuildingContextPanel.Find("FoodPerCitizenText").GetComponent<TMP_Text>();

        tradeVehicleStopButton = tradeVehicleContextPanel.Find("CDButton").GetComponent<Button>(); ;
        tradeVechicleStopCostText = tradeVehicleContextPanel.transform.Find("CostText").GetComponent<TMP_Text>();
    }

    public bool OpenContext(GameObject obj)
    {
        CloseAll();
        var selectable = obj.GetComponent<ISelectable>();
        if (selectable == null) return false;

        transform.position = obj.transform.position + new Vector3(0, offsetY, 0);
        canvas.enabled = true;
        TradeVehicle vehicle = selectable as TradeVehicle;
        if (vehicle != null)
        {
            //You shouldnt be able to hold up your own vehicle
            if (selectable.GetTeam() == GameManager.instance.localPlayer.team)
                return false;

            OpenContext(vehicle);
            return true;
        }
        Building building = selectable as Building;
        if (building != null)
        {
            if (selectable.GetTeam() != GameManager.instance.localPlayer.team)
                return false;

            OpenContext(building);
            return true;
        }
        return false;
    }
    public void OpenContext(TradeVehicle vehicle)
    {
        tradeVehicleContextPanel.gameObject.SetActive(true);
        UpdateContextUi(vehicle);
    }

    public void OpenContext(Building building)
    {
        if (building is Mainbuilding)
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
        buildingContextUiLevelCostText.text = building.GetLevelCostString();
        buildingContextUiLevelUpButton.interactable = building.CheckCanLevelUp();
    }

    public void UpdateContextUi(Mainbuilding mainbuilding)
    {
        mainbuildingContextUiTaxesSlider.maxValue = mainbuilding.maxTaxes;
        mainbuildingContextUiTaxesText.text = $"Taxes:  {mainbuilding.Taxes}/{mainbuilding.maxTaxes} per citizen";
        mainbuildingContextUiText.text = mainbuilding.GetStats();
        mainbuildingContextUiTaxesSlider.value = mainbuilding.Taxes;
        mainBuildingContextUiFoodText.text = $"{mainbuilding.foodUsePerDayPerCitizen} food per citizen per day";
    }

    public void UpdateContextUi(TradeVehicle vehicle)
    {
        tradeVehicleStopButton.interactable = ResourceUiManager.instance.activeResourceMan.GetAmount(resource.gold) >= vehicle.holdUpCost;
        tradeVechicleStopCostText.text = vehicle.holdUpCost.ToString();
    }

    public void CloseAll()
    {
        mainbuildingContextPanel.gameObject.SetActive(false);
        buildingContextPanel.gameObject.SetActive(false);
        tradeVehicleContextPanel.gameObject.SetActive(false);
        canvas.enabled = false;
    }
}
