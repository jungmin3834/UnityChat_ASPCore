using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public int MessageLimit = 5;
    public GameObject ChatPanel, TextObject;
    public InputField InputText;
    [SerializeField]
    Queue<Message> _messageList = new Queue<Message>();
    // Start is called before the first frame update

    private void Update()
    {
       Stack<Chat> chatList = ServerManager.instance.GetMessageFromServer();
       if(chatList != null)
        {
            for(int i = chatList.Count;i > 0;i--)
            {
                SendMessageToChat(chatList.Peek());
                chatList.Pop();
            }
        }
    }

    public void Ui_SendMessage()
    {
        if (InputText.text.Trim().Equals("") == true)
            return;

        ServerManager.instance.SendMessageToServer(this,InputText.text);
        InputText.text = String.Format("");
    }


    public void SendMessageToChat(Chat chat)
    {
        //_messageList의 Count 값이 MessageLimit 값 초과시 기존 오브젝트 풀링을 이용하여 재활용.
        if (_messageList.Count >= MessageLimit)
            MakeMessage(_messageList.Dequeue(), chat);
        else
            MakeMessage(new Message(Instantiate(TextObject,ChatPanel.transform), chat), chat);
        
 
    }

    private void MakeMessage(Message message , Chat chat)
    {
       
        message.SetTextMessage(chat);
        _messageList.Enqueue(message);
    }

}


public class Chat
{
    public string UserName { get; set; }
    public string UserText { get; set; }

    public Chat(string userName, string userText)
    {
        UserName = userName;
        UserText = userText;
    }
}

[SerializeField]
public class Message
{
    private GameObject TextObject;
   
    public Message(GameObject messages, Chat text)
    {
        TextObject = messages;
        SetTextMessage(text);
    }

    public void SetTextMessage(Chat text)
    {
        TextObject.GetComponent<Text>().text = string.Format($"{text.UserName} : {text.UserText}");
    }
}
        