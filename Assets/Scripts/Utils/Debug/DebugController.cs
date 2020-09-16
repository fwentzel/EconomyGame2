using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DebugController : MonoBehaviour
{
    bool showConsole;
    bool showHelp;

    string input;
    Inputmaster inputMaster;

    public static DebugCommand<resource, int> ADD_RES;
    public static DebugCommand HELP;

    public List<object> commandList;

    private void Awake()
    {
        inputMaster = InputMasterManager.instance.inputMaster;
        inputMaster.Debug.ToggleDebug.performed += _ => OnToggleDebug();
        inputMaster.Debug.HandleInput.performed += _ => HandleInput();

    
        ADD_RES = new DebugCommand<resource, int>("add_res", "Add specified res amount. Possible resources: food, loyalty, gold, citizens, stone ", "add_res <resource><amount>", (x, y) =>
       {
           ResourceUiManager.instance.activeResourceMan.ChangeRessourceAmount(x, y);
       });
        HELP = new DebugCommand("help", "show help and description of all available commands", "help", () => showHelp = true);

        commandList = new List<object>{
        
        ADD_RES,
        HELP
    };

    }

    public void OnToggleDebug()
    {
        if (showConsole)
        {
            showConsole = false;
            inputMaster.Camera.Enable();
            inputMaster.Menus.Enable();
        }
        else
        {
            showConsole = true;
            inputMaster.Camera.Disable();
            inputMaster.Menus.Disable();
        }
    }
    private void HandleInput()
    {
        string[] properties = input.Split(' ');
        for (int i = 0; i < commandList.Count; i++)
        {

            DebugCommandBase commandBase = commandList[i] as DebugCommandBase;
            if (input.Contains(commandBase.commandID))
            {

                if (commandList[i] is DebugCommand command)
                {
                    command.Invoke();
                }
                else if (commandList[i] is DebugCommand<int> commandInt)
                {
                    commandInt.Invoke(int.Parse(properties[1]));
                }
                else if (commandList[i] is DebugCommand<resource, int> commandResInt)
                {
                    resource _resource;
                    if (resource.TryParse(properties[1], out _resource))
                    {
                        //correct resource Input
                        commandResInt.Invoke(_resource, int.Parse(properties[2]));

                    }

                }
            }
        }
    }

Vector2 scroll;
    private void OnGUI()
    {
        if (!showConsole) return;

        float y = 0f;

        if(showHelp){
            GUI.Box(new Rect(0,y,Screen.width,100),"");

            Rect viewport = new Rect(0,0,Screen.width-30,20*commandList.Count);

            scroll=GUI.BeginScrollView(new Rect(0,y+5f,Screen.width,90),scroll,viewport);

            for (int i = 0; i < commandList.Count; i++)
            {
                DebugCommandBase command = commandList[i] as DebugCommandBase;
                string label=$"{command.commandFormat} - {command.commandDescription}";
                Rect labelRect=new Rect(5,20*i,viewport.width-100,20);
                GUI.Label(labelRect,label);
            }
            GUI.EndScrollView();
            y+=100;
        }

        GUI.Box(new Rect(0, y, Screen.width, 30), "");
        GUI.backgroundColor = new Color(0, 0, 0, 0);
        input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), input);
    }
    private void OnEnable()
    {
        inputMaster.Debug.Enable();
    }
    private void OnDisable()
    {
        inputMaster.Debug.Disable();
    }
}
