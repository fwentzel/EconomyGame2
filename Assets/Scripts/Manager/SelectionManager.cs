using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionManager : MonoBehaviour
{
	public static SelectionManager instance { get; private set; }
	public event Action OnSelectionChange = delegate { };

	public GameObject selectedObject
	{
		get => SelectedObject;
		set => SetSelectedObject(value);
	}


	private GameObject SelectedObject;
	
	Building building;

	private void Awake()
	{
		//singleton Check
		if (instance == null)
			instance = this;
		else
			Destroy(this);
	}

	private void SetSelectedObject(GameObject value)
	{
		SelectedObject = value;
		OnSelectionChange?.Invoke();	
	}

	public void Deselect()
	{
		SelectedObject = null;
		ContextUiManager.instance.CloseContextMenus();
		OnSelectionChange?.Invoke();
	}

}
