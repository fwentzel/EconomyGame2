using UnityEngine;

public class UiActionHandler : MonoBehaviour
{
	public void DestroySelected()
	{
		Building buildingToDestroy = SelectionManager.instance.SelectedObject.GetComponent<Building>();
		if (buildingToDestroy != null)
		{
			buildingToDestroy.DestroyBuilding();
			SelectionManager.instance.SelectedObject = null;
			UiManager.instance.CloseAll();
		}
	}

	public void LevelUpSelected()
	{
		Building buildingToLevelUp = SelectionManager.instance.SelectedObject.GetComponent<Building>();
		if (buildingToLevelUp != null)
		{
			buildingToLevelUp.LevelUp();
			UiManager.instance.UpdateContextUi(buildingToLevelUp);
		}
	}

	public void ChangeTaxes(float value)
	{
		if (SelectionManager.instance.SelectedObject == null)
			return;
		Mainbuilding mainbuilding = SelectionManager.instance.SelectedObject.GetComponent<Mainbuilding>();
		if (mainbuilding != null)
		{
			mainbuilding.Taxes = (int)value;
			UiManager.instance.UpdateContextUi(mainbuilding);
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
