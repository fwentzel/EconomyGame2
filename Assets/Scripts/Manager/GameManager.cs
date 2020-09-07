
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public event Action OnCalculateIntervall = delegate { };
    public event Action OnGameStart = delegate { };
    public int dayIndex = 0;
    public int calcResourceIntervall = 10;

    public Player[] players;
    public Player localPlayer;

    //Debugging 
    public bool showAiLog = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);




    }
    private void Update()
    {
        if (Keyboard.current.spaceKey.isPressed)
        {
            Time.timeScale = 5;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void RpcSetupMainBuildingPlayer()
    {
        Mainbuilding[] mainbuildings = FindObjectsOfType<Mainbuilding>();
        for (int i = 0; i < players.Length; i++)
        {
            
                mainbuildings[i].SetupMainBuilding(players[i].isAi);
                players[i].SetMainBuilding(mainbuildings[i]);

            
        }
        CityResourceLookup.instance.PopulateResourceManagers(mainbuildings.Length);
    }

    void RpcStartInvokeCalcIntervall()
    {
        OnGameStart?.Invoke();
        InvokeRepeating("InvokeCalculateResource", 0, calcResourceIntervall);
        FindObjectOfType<GameTimer>().StartTimer(calcResourceIntervall);
    }

    private void InvokeCalculateResource()
    {
        OnCalculateIntervall?.Invoke();
        dayIndex++;
    }

    public void StartGame()
    {
        RpcSetupMainBuildingPlayer();
        PlacementController.instance.SetupGridParameter();
        TradeManager.instance.StartTradeOffer();
        BuildUi.instance.GenerateBuildMenu();

        RpcStartInvokeCalcIntervall();
    }


}
