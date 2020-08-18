using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MessageSystem : MonoBehaviour
{
    public static MessageSystem instance;
    [SerializeField] GameObject messagePanel = null;
    [SerializeField] Transform contentParent = null;
    [SerializeField] GameObject textPrefab = null;
    WaitForSeconds cachedWait = new WaitForSeconds(5);

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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Message("HAU REEEIINNN!",Color.red);
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
        StartCoroutine(hideChat());
    }
    public void Message(string message, Color color = default)
    {
        GameObject newChatMessage = Instantiate(textPrefab, contentParent);
        TMP_Text chatText = newChatMessage.GetComponent<TMP_Text>();

        chatText.text = message;
        
        chatText.color = color== default ? Color.black : color; 
        showChat();

    }
}
