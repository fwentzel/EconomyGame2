using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UiManager : MonoBehaviour
{
    public static UiManager instance { get; private set; }

    Transform settingsPanel;
    Transform traderPanel;
    Transform menuPanel;
    Transform endgamePanel;
    Transform scoreboardPanel;
    GameObject newTradesTimerParent;
    TMP_Text newTradesInText;
    Image newTradesInImage;

    Inputmaster input;
    private void Awake()
    {
        //singleton Check
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        SetupUiElements();
        CloseAll();


        TradeManager.instance.OnGenerateNewTrades += (int arrivalIn) => StartCoroutine(StartNewTradeTimerCoroutine(arrivalIn));
    }

    private void Start()
    {
        input = InputMasterManager.instance.inputMaster;
        input.Menus.Trader.performed += _ => OpenMenu(traderPanel.gameObject);
        input.Menus.Scoreboard.started += _ => OpenMenu(scoreboardPanel.gameObject);
        input.Menus.Scoreboard.canceled += _ => CloseAll();
        input.Menus.Menu.performed += _ => OpenMenu(menuPanel.gameObject);
        GameManager.instance.OnGameEnd += () => OpenMenu(endgamePanel.gameObject);
    }

    private void SetupUiElements()
    {
        menuPanel = transform.Find("MenuPanel");
        endgamePanel = transform.Find("EndgamePanel");
        settingsPanel = transform.Find("SettingsPanel");
        scoreboardPanel = transform.Find("ScoreboardPanel");
        traderPanel = transform.Find("TraderPanel");

        newTradesTimerParent = traderPanel.transform.Find("Timer").gameObject;
        newTradesInText = newTradesTimerParent.transform.Find("NewTradesTimerText").GetComponent<TMP_Text>();
        newTradesInImage = newTradesTimerParent.transform.Find("NewTradeTimerForeground").GetComponent<Image>();
    }
    public void OpenMenu(GameObject menuToOpen)
    {
        if (menuToOpen == null)
        {
            OpenMenu(menuPanel.gameObject);
        }
        bool wasActive = menuToOpen.activeSelf;
        CloseAll();
        if (wasActive == false)
        {
            menuToOpen.SetActive(true);
        }

    }

    public void CloseAll()
    {
        traderPanel.gameObject.SetActive(false);
        settingsPanel.gameObject.SetActive(false);
        menuPanel.gameObject.SetActive(false);
        scoreboardPanel.gameObject.SetActive(false);
        endgamePanel.gameObject.SetActive(false);
    }

    private IEnumerator StartNewTradeTimerCoroutine(int arrivalIn)
    {
        float normalizedTime = arrivalIn;
        newTradesTimerParent.SetActive(true);

        while (normalizedTime >= 0)
        {
            newTradesInText.text = Mathf.CeilToInt(normalizedTime) + " seconds!";
            newTradesInImage.fillAmount = normalizedTime / arrivalIn;
            normalizedTime -= Time.deltaTime;
            yield return null;
        }
        newTradesTimerParent.SetActive(false);
    }

}