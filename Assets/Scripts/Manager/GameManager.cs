
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public event Action OnCalculateIntervall = delegate { };
    public event Action OnGameStart = delegate { };
    public event Action OnGameEnd = delegate { };
    public int dayIndex = 0;
    public int calcResourceIntervall = 5;

    public Player[] players;
    public Player localPlayer;
    public bool didPlayerWin { get; private set; } = false;

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
            Time.timeScale = 5;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void setPlayerGameOver(Mainbuilding mb)
    {
        foreach (var player in players)
        {
            if (player.mainbuilding == mb)
            {
                if (!player.isAi)
                {
                    MessageSystem.instance.Message("YOU LOST!", Color.red);

                    OnGameEnd?.Invoke();
                    CancelInvoke();
                }
                else
                {
                    gameOverPlayers++;
                    if (gameOverPlayers == players.Length - 1)//TODO, unsicher
                    {
                        MessageSystem.instance.Message("YOU WIN!", Color.green);
                        didPlayerWin = true;
                        OnGameEnd?.Invoke();
                        CancelInvoke();
                    }
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
        }
    }

    void RpcStartInvokeCalcIntervall()
    {
        
        OnGameStart?.Invoke();
        InvokeRepeating("InvokeCalculateResource", calcResourceIntervall, calcResourceIntervall);
    }

    private void InvokeCalculateResource()
    {
        OnCalculateIntervall?.Invoke();
        dayIndex++;
    }

    public void StartGame()
    {
        RpcSetupMainBuildingPlayer();
        RpcStartInvokeCalcIntervall();
    }


}
