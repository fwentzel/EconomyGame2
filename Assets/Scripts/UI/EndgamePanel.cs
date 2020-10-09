using UnityEngine;
using TMPro;

public class EndgamePanel : MonoBehaviour
{
    [SerializeField] Color winColor = default;
    [SerializeField] Color looseColor= default;
    private void Start()
    {
        TMP_Text text = transform.Find("Title").GetComponent<TMP_Text>();
        text.text = GameManager.instance.didPlayerWin ? "YOU WIN!" : "GAME OVER";
        text.color = GameManager.instance.didPlayerWin ? winColor : looseColor;
    }
}
