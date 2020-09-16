using UnityEngine;

public class InputMasterManager : MonoBehaviour
{
    public Inputmaster inputMaster{get;private set;}
    public static InputMasterManager instance;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        inputMaster = new Inputmaster();
    }
}