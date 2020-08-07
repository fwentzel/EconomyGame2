using UnityEngine;

public class UiActionHandler : MonoBehaviour
{
	public void DestroySelected()
	{
		Building buildingToDestroy = SelectionManager.instance.selectedObject.GetComponent<Building>();
		if (buildingToDestroy != null)
		{
			buildingToDestroy.DestroyBuilding();
			SelectionManager.instance.Deselect();
			UiManager.instance.CloseAll();
		}
	}

	public void LevelUpSelected()
	{
		Building buildingToLevelUp = SelectionManager.instance.selectedObject.GetComponent<Building>();
		if (buildingToLevelUp != null)
		{
			buildingToLevelUp.LevelUp();
			BuildingContextUiManager.instance.UpdateContextUi(buildingToLevelUp);
		}
	}

	public void ChangeTaxes(float value)
	{
		Mainbuilding mainbuilding = SelectionManager.instance.selectedObject.GetComponent<Mainbuilding>();
		if (mainbuilding != null)
		{
			mainbuilding.Taxes = (int)value;
			BuildingContextUiManager.instance.UpdateContextUi(mainbuilding);
		}
	}

	public void ExitApplication()
	{
		print("QUIT!");
		Application.Quit();
	}

	public void Disconnect()
	{
		print("Disconnected!");
	}

	public void BackToMain()
	{
		UiManager.instance.OpenMenu(null);
	}
}
