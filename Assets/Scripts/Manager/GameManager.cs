
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
    public int calcResourceIntervall = 5;

    public Player[] players;
    public Player localPlayer;

    int gameOverPlayers = 0;

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
        if (Keyboard.current.fKey.isPressed)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 10;
        }
    }

    public void setPlayerGameOver(Mainbuilding mb)
    {
        foreach (var player in players)
        {
            if (player.mainbuilding == mb)
            {
                gameOverPlayers++;
                if (gameOverPlayers == players.Length - 1)//TODO, unsicher
                { 
                    MessageSystem.instance.Message("GAME IS OVER!",Color.green);
                    CancelInvoke();
                }
            }
        }
    }

    public void RpcSetupMainBuildingPlayer()
    {
        Mainbuilding[] mainbuildings = FindObjectsOfType<Mainbuilding>();
        for (int i = 0; i < players.Length; i++)
        {

            mainbuildings[i].SetupMainBuilding(players[i].isAi);
            players[i].SetMainBuilding(mainbuildings[i]);
             FindObjectOfType<GameTimer>().isRunning=false;


        }
        CityResourceLookup.instance.PopulateResourceManagers(mainbuildings.Length);
    }

    void RpcStartInvokeCalcIntervall()
    {
        OnGameStart?.Invoke();
        InvokeRepeating("InvokeCalculateResource", calcResourceIntervall, calcResourceIntervall);
        ResourceUiManager.instance.UpdateRessourceUI();
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
