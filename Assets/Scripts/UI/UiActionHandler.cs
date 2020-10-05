using UnityEngine;
using UnityEngine.EventSystems;

public class UiActionHandler : MonoBehaviour
{
    public static void HoldUpSelected(ButtonCD buttonCD)
    {
        TradeVehicle tradeVehicle = SelectionManager.instance.selectedObject.GetComponent<TradeVehicle>();

        if (tradeVehicle != null)
        {
            buttonCD.SetUp(tradeVehicle.holdUpDuration);
            buttonCD.OnCDFinished += delegate () { ContextUiManager.instance.UpdateContextUi(tradeVehicle); };
            tradeVehicle.HoldUp();

        }
    }
    public static void DestroySelected()
    {
        Building buildingToDestroy = SelectionManager.instance.selectedObject.GetComponent<Building>();
        if (buildingToDestroy != null)
        {
            buildingToDestroy.DestroyBuilding();
            SelectionManager.instance.Deselect();
            UiManager.instance.CloseAll();
        }
    }

    public static void LevelUpSelected()
    {
        Building buildingToLevelUp = SelectionManager.instance.selectedObject.GetComponent<Building>();
        if (buildingToLevelUp != null)
        {
            buildingToLevelUp.LevelUp();
            ContextUiManager.instance.UpdateContextUi(buildingToLevelUp);
        }
    }

    public static void ChangeTaxes(float value)
    {

        Mainbuilding mainbuilding = SelectionManager.instance.selectedObject.GetComponent<Mainbuilding>();
        if (mainbuilding != null)
        {
            ContextUiManager.instance.UpdateContextUi(mainbuilding);
            if (!ContextUiManager.instance.isSameTeam)//Slider Method shouldhnt be called TODO 
                return;
            mainbuilding.Taxes = (int)value;
        }
    }

    public static void TakeCitizens()
    {

        Mainbuilding mainbuilding = SelectionManager.instance.selectedObject.GetComponent<Mainbuilding>();
        if (mainbuilding != null)
        {
            CitizenManager.instance.TakeOverCitizen(mainbuilding.resourceManager, ResourceUiManager.instance.activeResourceMan, (int)ContextUiManager.instance.mainbuildingContextUiSlider.value);
            ContextUiManager.instance.mainbuildingContextUiConfirmButton.GetComponent<ButtonCD>().SetUp(5);
            ContextUiManager.instance.UpdateContextUi(mainbuilding);
        }


    }

    public static void ExitApplication()
    {
        print("QUIT!");
        Application.Quit();
    }

    public static void Disconnect()
    {
        print("Disconnected!");
    }

    public static void BackToMain()
    {
        UiManager.instance.OpenMenu(null);
    }
}
