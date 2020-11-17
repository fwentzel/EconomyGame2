using UnityEngine;

public class InputMasterManager : MonoBehaviour
{
    public static InputMasterManager instance { get; private set; }
    public Inputmaster inputMaster { get; private set; }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        inputMaster = new Inputmaster();
    }

    private void Start()
    {
        if(GameManager.instance!=null)
        GameManager.instance.OnGameStart += () => inputMaster.Menus.Enable();
        else
        {
            inputMaster.Menus.Enable();
        }
    }
}
