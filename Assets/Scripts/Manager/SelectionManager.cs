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
        
        mouse = Mouse.current;
    }
    private void Start() {
        input = InputMasterManager.instance.inputMaster;
        input.Selection.Click.started += _ => GetObjectFromClick();
        input.Selection.Enable();
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
    private void SetSelectedObject(GameObject obj)
    {
        SelectedObject = obj;
        

    }

    public void Deselect()
    {
        SelectedObject = null;
        ContextUiManager.instance.CloseAll();
        OnSelectionChange?.Invoke();
    }

}
