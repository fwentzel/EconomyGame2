using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ContextUiManager : MonoBehaviour
{
    public static ContextUiManager instance { get; private set; }

    Transform buildingContextPanel = null;
    Text buildingContextUiText;
    Button buildingContextUiLevelUpButton;
    Text buildingContextUiLevelCostText;

    Transform mainbuildingContextPanel = null;
    Text mainbuildingContextUiText;
    Text mainbuildingContextUiTaxesText;
    Slider mainbuildingContextUiTaxesSlider;

    Transform tradeVehicleContextPanel = null;
    Button tradeVehicleStopButton;
    Text tradeVechicleStopCostText;

    // Use this for initialization
    private void Awake()
    {
        //singleton Check
        if (instance == null)
            instance = this;
        else
            Destroy(this);
        SetupUiElements();
        CloseContextMenus();
    }

    private void Start()
    {
        ResourceUiManager.instance.activeResourceMan.OnResourceChange += TriggerUpdateContext;
    }

    void TriggerUpdateContext()
    {
        GameObject selected = SelectionManager.instance.selectedObject;
        if (selected != null)
        {
            Building bld = selected.GetComponent<Building>();
            TradeVehicle vhcl = selected.GetComponent<TradeVehicle>();
            if (bld != null)
                UpdateContextUi(bld);
            else if (vhcl != null)
                UpdateContextUi(vhcl);
        }

    }

    private void SetupUiElements()
    {
        buildingContextPanel = transform.Find("BuildingContextPanel");
        mainbuildingContextPanel = transform.Find("MainBuildingContextPanel");
        tradeVehicleContextPanel = transform.Find("TraderVehicleContextPanel");

        buildingContextUiText = buildingContextPanel.GetChild(0).Find("ContextText").GetComponent<Text>();
        buildingContextUiLevelUpButton = buildingContextPanel.GetChild(0).Find("LevelUpButton").GetComponent<Button>();
        buildingContextUiLevelCostText = buildingContextUiLevelUpButton.GetComponentInChildren<Text>();

        mainbuildingContextUiText = mainbuildingContextPanel.GetChild(0).Find("ContextText").GetComponent<Text>();
        mainbuildingContextUiTaxesText = mainbuildingContextPanel.GetChild(0).Find("TaxesText").GetComponent<Text>();
        mainbuildingContextUiTaxesSlider = mainbuildingContextPanel.GetChild(0).GetComponentInChildren<Slider>();

        tradeVehicleStopButton = tradeVehicleContextPanel.GetChild(0).Find("HoldUpButton").GetComponent<Button>(); ;
        tradeVechicleStopCostText = tradeVehicleStopButton.GetComponentInChildren<Text>();
    }

    public void OpenContext(GameObject obj)
    {
        TradeVehicle vehicle = obj.GetComponent<ISelectable>() as TradeVehicle;
        if (vehicle != null)
            OpenContext(vehicle);
        Building building = obj.GetComponent<ISelectable>() as Building;
        if (building != null)
            OpenContext(building);
    }
    public void OpenContext(TradeVehicle vehicle)
    {
        CloseContextMenus();
        tradeVehicleContextPanel.gameObject.SetActive(true);
        UpdateContextUi(vehicle);
    }

    public void OpenContext(Building building)
    {
        CloseContextMenus();
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
        buildingContextUiLevelCostText.text = building.levelCost.ToString();
        buildingContextUiLevelUpButton.interactable = building.CheckCanLevelUp();
    }

    public void UpdateContextUi(Mainbuilding mainbuilding)
    {
        mainbuildingContextUiTaxesText.text = "Taxes: " + mainbuilding.Taxes + " /10 per citizen";
        mainbuildingContextUiText.text = mainbuilding.GetStats();
        mainbuildingContextUiTaxesSlider.value = mainbuilding.Taxes;
    }

    public void UpdateContextUi(TradeVehicle vehicle)
    {
        tradeVehicleStopButton.interactable = ResourceUiManager.instance.activeResourceMan.GetAmount(resource.gold) >= vehicle.holdUpCost;
        tradeVechicleStopCostText.text = vehicle.holdUpCost.ToString();
    }

    public void CloseContextMenus()
    {
        mainbuildingContextPanel.gameObject.SetActive(false);
        buildingContextPanel.gameObject.SetActive(false);
        tradeVehicleContextPanel.gameObject.SetActive(false);
    }
}
