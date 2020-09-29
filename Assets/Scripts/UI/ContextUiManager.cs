using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ContextUiManager : MonoBehaviour
{
    public static ContextUiManager instance { get; private set; }

    [SerializeField] int offsetY = 50;
    Transform buildingContextPanel = null;
    TMP_Text buildingContextUiText;
    Button buildingContextUiLevelUpButton;
    TMP_Text buildingContextUiLevelCostText;

    Transform mainbuildingContextPanel = null;
    TMP_Text mainbuildingContextUiText;
    TMP_Text mainbuildingContextUiTaxesText;
    Slider mainbuildingContextUiTaxesSlider;

    Transform tradeVehicleContextPanel = null;
    Button tradeVehicleStopButton;
    TMP_Text tradeVechicleStopCostText;

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
        GameManager.instance.OnGameStart += ()=>ResourceUiManager.instance.activeResourceMan.OnResourceChange += TriggerUpdateContext;

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
        buildingContextPanel = transform.Find("BuildingContextPanel");
        mainbuildingContextPanel = transform.Find("MainBuildingContextPanel");
        tradeVehicleContextPanel = transform.Find("TraderVehicleContextPanel");

        buildingContextUiText = buildingContextPanel.GetChild(0).Find("ContextText").GetComponent<TMP_Text>();
        buildingContextUiLevelUpButton = buildingContextPanel.GetChild(0).Find("LevelUpButton").GetComponent<Button>();
        buildingContextUiLevelCostText = buildingContextUiLevelUpButton.GetComponentInChildren<TMP_Text>();

        mainbuildingContextUiText = mainbuildingContextPanel.GetChild(0).Find("ContextText").GetComponent<TMP_Text>();
        mainbuildingContextUiTaxesText = mainbuildingContextPanel.GetChild(0).Find("TaxesText").GetComponent<TMP_Text>();
        mainbuildingContextUiTaxesSlider = mainbuildingContextPanel.GetChild(0).GetComponentInChildren<Slider>();

        tradeVehicleStopButton = tradeVehicleContextPanel.GetChild(0).Find("HoldUpButton").GetComponent<Button>(); ;
        tradeVechicleStopCostText = tradeVehicleStopButton.GetComponentInChildren<TMP_Text>();
    }

    public bool OpenContext(GameObject obj)
    {
        CloseAll();
        var selectable = obj.GetComponent<ISelectable>();
        transform.position = obj.transform.position + new Vector3(0, offsetY, 0);

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
    }
}
