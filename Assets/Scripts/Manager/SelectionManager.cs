using UnityEngine;

public class SelectionManager : MonoBehaviour
{
	public static SelectionManager instance { get; private set; }
	public GameObject SelectedObject { get => selectedObject; set => NewSelectedObject(value); }

	private GameObject selectedObject = null;
	Building building;

	private void Awake()
	{
		//singleton Check
		if (instance == null)
			instance = this;
		else
			Destroy(this);
	}

	void NewSelectedObject(GameObject newObj)
	{
		selectedObject = newObj;
		UiManager.instance.OpenContext(newObj.GetComponent<Building>());
	}
	
}
