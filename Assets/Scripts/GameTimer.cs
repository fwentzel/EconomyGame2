using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    Image timerImage;

    public float fillAmount = 0;
    public float dayLength = 2.0f;
    public bool isRunning = false;

    Color bgColor;
    Color fgColor;

    Image backgroundImage;
    bool foregroundIsWhite = true;
    private void Awake()
    {
        timerImage = GetComponent<Image>();
        backgroundImage = transform.parent.GetChild(0).GetComponent<Image>();
        bgColor = backgroundImage.color;
        fgColor = timerImage.color;
    }
    private void Start()
    {
        GameManager.instance.OnGameStart += StartTimer;
        GameManager.instance.OnGameEnd += () => isRunning = false;
    }

    public void StartTimer()
    {
        this.dayLength = GameManager.instance.calcResourceIntervall;
        isRunning = true;
    }

    void Update()
    {
        if (isRunning)
        {//Reduce fill amount 
            fillAmount += 1.0f / dayLength * Time.deltaTime;

            if (fillAmount >= 1)
            {
                timerImage.color = foregroundIsWhite ? bgColor : fgColor;
                backgroundImage.color = foregroundIsWhite ? fgColor : bgColor;
                foregroundIsWhite = !foregroundIsWhite;
                fillAmount = 0;
            }
        }
        timerImage.fillAmount = fillAmount;

    }
}
