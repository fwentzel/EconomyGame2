using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

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

    Inputmaster input;
    Mouse mouse;

    private void Awake()
    {
        //singleton Check
        if (instance == null)
            instance = this;
        else
            Destroy(this);
        input = new Inputmaster();
        input.Selection.Click.canceled += _ => GetObjectFromClick();
        mouse = Mouse.current;
    }
    private void OnEnable()
    {
        input.Enable();
    }

    private void GetObjectFromClick()
    {
        if (EventSystem.current.IsPointerOverGameObject() || PlacementController.instance.isPlacing)
            return;
        GameObject obj = Utils.GetObjectAtMousePos(mouse.position.ReadValue());
        if (obj == null)
            return;
        selectedObject = obj;

        if (ContextUiManager.instance.OpenContext(obj))//false, if there is no type for selection or trying to select Obj from other team
        {
            OnSelectionChange?.Invoke();
        }


    }
    private void SetSelectedObject(GameObject value)
    {
        SelectedObject = value;

    }

    public void Deselect()
    {
        SelectedObject = null;
        ContextUiManager.instance.CloseAll();
        OnSelectionChange?.Invoke();
    }

}
