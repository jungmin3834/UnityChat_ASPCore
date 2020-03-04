using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int MessageLimit = 5;
    public GameObject ChatPanel, TextObject;

    [SerializeField]
    Queue<Message> _messageList = new Queue<Message>();
    // Start is called before the first frame update
    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            SendMessageToChat("Input my Message! Hello World!");
    }

    public void SendMessageToChat(string text)
    {
        //_messageList의 Count 값이 MessageLimit 값 초과시 기존 오브젝트 풀링을 이용하여 재활용.
        if (_messageList.Count >= MessageLimit)
            MakeMessage(_messageList.Dequeue(), text);
        else
            MakeMessage(new Message(Instantiate(TextObject, ChatPanel.transform),text), text);
    }

    private void MakeMessage(Message message , string text)
    {
        message.SetTextMessage(text);
        _messageList.Enqueue(message);
    }

}


[SerializeField]
public class Message
{
    private GameObject TextObject;

    public Message(GameObject messages, string text)
    {
        TextObject = messages;
        SetTextMessage(text);
    }

    public void SetTextMessage(string text)
    {
        TextObject.GetComponent<Text>().text = text;
    }
}
        