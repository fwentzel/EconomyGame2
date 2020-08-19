using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager instance { get; private set; }

    Transform settingsPanel;
    Transform traderPanel;
    Transform menuPanel;
    Transform scoreboardPanel;
    GameObject newTradesTimerParent;
    Text newTradesInText;
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
        input = new Inputmaster();
        
        TradeManager.instance.OnGenerateNewTrades += (int arrivalIn) => StartCoroutine(StartNewTradeTimerCoroutine(arrivalIn));
    }

    private void OnEnable() {
        input.Enable();
    }
    private void Start() {
        CloseAll();
        input.Menus.Trader.performed += _ => OpenMenu(traderPanel.gameObject);
        input.Menus.Scoreboard.started += _ => OpenMenu(scoreboardPanel.gameObject);
        input.Menus.Scoreboard.canceled += _ => CloseAll();
        input.Menus.Menu.performed += _ => OpenMenu(menuPanel.gameObject);

    }

    private void SetupUiElements()
    {
        menuPanel = transform.Find("MenuPanel");
        settingsPanel = transform.Find("SettingsPanel");
        scoreboardPanel=transform.Find("ScoreboardPanel");
        traderPanel = transform.Find("TraderPanel");

        newTradesTimerParent = traderPanel.transform.Find("Timer").gameObject;
        newTradesInText = newTradesTimerParent.transform.Find("NewTradesTimerText").GetComponent<Text>();
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
            menuToOpen.SetActive(true);
    }

    public void CloseAll()
    {
        traderPanel.gameObject.SetActive(false);
        settingsPanel.gameObject.SetActive(false);
        menuPanel.gameObject.SetActive(false);
        scoreboardPanel.gameObject.SetActive(false);
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