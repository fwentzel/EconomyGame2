using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class MessageSystem : MonoBehaviour
{
    public static MessageSystem instance;
    [SerializeField] GameObject messagePanel = null;
    [SerializeField] Transform contentParent = null;
    [SerializeField] GameObject messagePrefab = null;
    [SerializeField] int maxMessages = 4;
    Scrollbar scrollbar => messagePanel.GetComponent<ScrollRect>().verticalScrollbar;
    int msgCount = 0;
    WaitForSeconds cachedWait = new WaitForSeconds(5);
    Inputmaster input;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Debug.LogWarning("more then one Messagesystem active! Destroying this");
            Destroy(this);
        }
    }

    private void Start()
    {
        messagePanel.SetActive(false);
        input=InputMasterManager.instance.inputMaster;
    }

    private void Update()
    {
        
        if (input.Menus.enabled&& Keyboard.current.enterKey.wasReleasedThisFrame)
        {
            // Message("BRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRR");
            showChat();
        }
    }

    IEnumerator hideChat()
    {
        yield return cachedWait;
        messagePanel.SetActive(false);
    }

    void showChat()
    {
        StopAllCoroutines();
        messagePanel.SetActive(true);
        scrollbar.value = 0;
        StartCoroutine(hideChat());
    }
    TMP_Text chatText;
    public void Message(string message, Color color = default)
    {
        GameObject newChatMessage = null;
        msgCount++;
        if (msgCount <= maxMessages)
        {
            newChatMessage = Instantiate(messagePrefab, contentParent);
        }
        else
        {
            Transform t = contentParent.GetChild(0);
            newChatMessage = t.gameObject;
            t.SetSiblingIndex(contentParent.childCount);
        }

        chatText = newChatMessage.GetComponentInChildren<TMP_Text>();
        message = "[" + GameManager.instance.dayIndex + "] " + message;
        chatText.text = message;
        chatText.color = color == default ? Color.black : color;
        showChat();
        //chatText.ForceMeshUpdate();
        // chatText.GetTextInfo(message);
        RectTransform rectTransform=newChatMessage.GetComponent<RectTransform>();
        StartCoroutine(ResizeMessage(rectTransform));
       
    }
    IEnumerator ResizeMessage(RectTransform rectTransform){

        yield return 0;
        
        rectTransform.sizeDelta=new Vector2(rectTransform.sizeDelta.x,chatText.preferredHeight);

    }
}
