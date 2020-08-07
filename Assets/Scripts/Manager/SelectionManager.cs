using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager instance { get; private set; }

    public GameObject selectedObject { get; private set; }
	public GameObject hoveredObject;
	Building building;

    private void Awake()
    {
        //singleton Check
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

	private void Update()
	{
		if (Input.GetMouseButtonDown(0) &&!EventSystem.current.IsPointerOverGameObject() )
		{
			if (hoveredObject != null)
				SelectHoveredObject();
			else
				Deselect();
		}
	}

	void SelectHoveredObject()
    {
        selectedObject = hoveredObject;
        building= selectedObject.GetComponent<Building>();
		BuildingContextUiManager.instance.OpenContext(building);
	}

	public void Deselect()
	{
		selectedObject = null;
		BuildingContextUiManager.instance.CloseContextMenus();
	}

}
