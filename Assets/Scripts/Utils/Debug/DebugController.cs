using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugController : MonoBehaviour
{
    bool showConsole;
    bool showHelp;

    string input;
    Inputmaster inputMaster;

    public static DebugCommand<resource, int> ADD_RES;
    public static DebugCommand HELP;
    public static DebugCommand<string> MESSAGE;
    public static DebugCommand<Trade, ResourceManager> TRADE;

    public List<object> commandList;



    Vector2 scroll;

    private void Awake()
    {
        MESSAGE = new DebugCommand<string>("msg", "Send specified Message", "msg <message>", (x) =>
       {
           MessageSystem.instance.Message(x);
       });

        ADD_RES = new DebugCommand<resource, int>("add_res", "Add specified res amount. Possible resources: food, loyalty, gold, citizens, stone ", "add_res <resource><amount>", (x, y) =>
       {
           ResourceUiManager.instance.activeResourceMan.ChangeRessourceAmount(x, y);
       });

        TRADE = new DebugCommand<Trade, ResourceManager>("trade", "Accept custom Trade for specic team. When not important, put letter like X.", "trade <team> <toTraderRes, toTraderAmount, fromTraderRes, fromtraderAmount, type> ", (x, y) =>
         {

             TradeManager.instance.AcceptTrade(x, y, isDebug: true);
         });
        HELP = new DebugCommand("help", "show help and description of all available commands", "help", () => showHelp = true);

        commandList = new List<object>{
        TRADE,
        ADD_RES,
        MESSAGE,
        HELP
    };

    }

    private void Start()
    {
        inputMaster = InputMasterManager.instance.inputMaster;
        inputMaster.Debug.ToggleDebug.performed += _ => OnToggleDebug();
        inputMaster.Debug.HandleInput.performed += _ => HandleInput();
        inputMaster.Debug.Enable();

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
        if (string.IsNullOrEmpty(input)) return;

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
                else if (commandList[i] is DebugCommand<string> commandString)
                {
                    commandString.Invoke(input);
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
                else if (commandList[i] is DebugCommand<Trade, ResourceManager> commandTrade)
                {
                    resource _resource;
                    tradeType tradeType;
                    int amount;

                    resource resTo = resource.TryParse(properties[2], out _resource) ? _resource : resource.gold;
                    resource resFrom = resource.TryParse(properties[4], out _resource) ? _resource : resource.gold;
                    Trade newTrade = new Trade
                    {
                        toTrader = TradeManager.instance.GetTradingResource(resTo),
                        toTraderAmount = int.TryParse(properties[3], out amount) ? amount : 0,

                        fromTrader = TradeManager.instance.GetTradingResource(resFrom),
                        fromTraderAmount = int.TryParse(properties[5], out amount) ? amount : 0,
                        // type = tradeType.ship
                        type = tradeType.TryParse(properties[6], out tradeType) ? tradeType : tradeType.ship

                    };

                    ResourceManager resourceManager = CitysMeanResource.instance.resourceManagers[(int.TryParse(properties[5], out amount) ? amount : 1) - 1];
                    if (resourceManager == null)
                        resourceManager = ResourceUiManager.instance.activeResourceMan;

                    commandTrade.Invoke(newTrade, resourceManager);
                }
            }
        }
        input = "";
    }


    private void OnGUI()
    {
        if (!showConsole) return;

        float y = 0f;

        if (showHelp)
        {
            GUI.Box(new Rect(0, y, Screen.width, 100), "");

            Rect viewport = new Rect(0, 0, Screen.width - 30, 20 * commandList.Count);

            scroll = GUI.BeginScrollView(new Rect(0, y + 5f, Screen.width, 90), scroll, viewport);

            for (int i = 0; i < commandList.Count; i++)
            {
                DebugCommandBase command = commandList[i] as DebugCommandBase;
                string label = $"{command.commandFormat} - {command.commandDescription}";
                Rect labelRect = new Rect(5, 20 * i, viewport.width - 100, 20);
                GUI.Label(labelRect, label);
            }
            GUI.EndScrollView();
            y += 100;
        }

        GUI.Box(new Rect(0, y, Screen.width, 30), "");
        GUI.backgroundColor = new Color(0, 0, 0, 0);
        GUI.SetNextControlName("MyTextField");
        input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), input);
        GUI.FocusControl("MyTextField");
        
    }
}
