using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionManager : MonoBehaviour
{
	public static SelectionManager instance { get; private set; }

	public GameObject selectedObject
	{
		get => SelectedObject;
		set => SetSelectedObject(value);
	}


	private GameObject SelectedObject;

	public Transform selectionMarker;
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
		if (value.GetComponent<Building>() != null)
		{
			selectionMarker.position = new Vector3(SelectedObject.transform.position.x, .03f, SelectedObject.transform.position.z);
			selectionMarker.gameObject.SetActive(true);
		}
	}

	public void Deselect()
	{
		SelectedObject = null;
		selectionMarker.gameObject.SetActive(false);
		ContextUiManager.instance.CloseContextMenus();
	}

}
