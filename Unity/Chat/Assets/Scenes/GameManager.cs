using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    int maxMessages = 25;
    public GameObject chatPanel, textObject;

    [SerializeField]
    List<Message> messageList = new List<Message>();
    // Start is called before the first frame update

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            SendMessageToChat("Input my Message! Hello World!");
    }

    public void SendMessageToChat(string text)
    {
        if (messageList.Count >= maxMessages)
            messageList.Remove(messageList[0]);

        Message newMessage = new Message();
        newMessage.text = text;
        newMessage.textObject = Instantiate(textObject,chatPanel.transform);
        newMessage.textObject.GetComponent<Text>().text = newMessage.text;
        messageList.Add(newMessage);
    }

}


[SerializeField]
public class Message
{
    public string text;
    public GameObject textObject;
}
        