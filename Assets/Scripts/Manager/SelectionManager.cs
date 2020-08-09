using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager instance { get; private set; }

	public GameObject selectedObject;
	Building building;

    private void Awake()
    {
        //singleton Check
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

	public void Deselect()
	{
		selectedObject = null;
		ContextUiManager.instance.CloseContextMenus();
	}

}
