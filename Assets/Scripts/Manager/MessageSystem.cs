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

    float lastMessageSent = 0;
    int i = 1;

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
            showChat();
        }
    }

    IEnumerator hideChat()
    {
        yield return new WaitForSeconds(5);
        messagePanel.SetActive(false);
    }

    void showChat()
    {
        StopCoroutine(hideChat());
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
