using UnityEngine;

public class UiActionHandler : MonoBehaviour
{

	public void DestroySelected()
	{
		if (SelectionManager.instance.selectedObject == null)
			return;

		Building buildingToDestroy = SelectionManager.instance.selectedObject.GetComponent<Building>();
		if (buildingToDestroy != null)
		{
			buildingToDestroy.DestroyBuilding();
			SelectionManager.instance.selectedObject = null;
			UiManager.instance.CloseAll();
		}

	}
	public void LevelUpSelected()
	{
		if (SelectionManager.instance.selectedObject == null)
			return;

		Building buildingToLevelUp = SelectionManager.instance.selectedObject.GetComponent<Building>();
		if (buildingToLevelUp != null)
		{
			buildingToLevelUp.LevelUp();
			buildingToLevelUp.UpdateContextUi();
		}
	}

	public void ChangeTaxes(float value)
	{
		if (SelectionManager.instance.selectedObject == null)
			return;
		MainBuilding mainBuilding = SelectionManager.instance.selectedObject.GetComponent<MainBuilding>();
		if (mainBuilding != null)
		{
			mainBuilding.Taxes = (int)value;
			UiManager.instance.mainBuildingContextUiTaxesText.text = "Taxes: " + value + " /10 per citizen";
		}
	}

	public void ExitApplication()
	{
		print("QUIT!");
		Application.Quit();
	}

	public void BackToMain()
	{
		UiManager.instance.OpenMenu(UiManager.instance.menuPanel);
	}
}
