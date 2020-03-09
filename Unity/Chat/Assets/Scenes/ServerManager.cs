using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;
using LitJson;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.SignalR.Protocol;
using System.Threading.Tasks;
using System;

public class ChatViewModel
{
    public string UserName { get; set; }

    public string UserText { get; set; }
}

public class ServerManager : MonoBehaviour
{
    private UiManager _uiMannager;
    private HubConnection _connection;
    public static ServerManager instance;
    private Stack<Chat> _chatList = new Stack<Chat>();


    public Stack<Chat> GetMessageFromServer()
    {
        if (_chatList.Count != 0)
        {
            return _chatList;
        }
        return null;
    }

    private void Start()
    {
        instance = this;
        _uiMannager = this.GetComponent<UiManager>();
        Connect();
    }

    private void Connect()
    {
        try
        {
            _connection = new HubConnectionBuilder()
           .WithUrl("https://localhost:44361/chat")
           .Build();

            Debug.Log("Starting connection...");

            _connection.On<string, string>("BroadcastMessage", (s1, s2) => OnBroadcastMessage(s1, s2));
            _connection.On<string>("ConnectionState", (s1) => OnConnectionMessage(s1));

            Task.Run(() => _connection.StartAsync());
            Task.Run(() => SendConnectionToserver("ConnectionList", DateTime.Today.ToLongTimeString()));
            Invoke("test",2);
           

        }
        catch (Exception ex)
        {
            Debug.Log($"Connection Error :  { ex.Message} ");
        }

        Debug.Log("Connection established.");
    }

    private void test()
    {
        Task.Run(() => SendConnectionToserver("JoinGroupList", ""));
    }

    private void DisConnect()
    {
        try
        {
            Task.Run(() =>  _connection.StopAsync());
        }
        catch(Exception ex)
        {
            Debug.Log($"Close Error : {ex.Message}");
        }
    }

    private void OnBroadcastMessage(string name ,string message)
    {
        _chatList.Push(new Chat(name, message));
    }

    private void OnConnectionMessage(string message)
    {
        _chatList.Push(new Chat("System", message));
    }

    public void SendMessageToServer(string text)
    {
        Task.Run(() => sendMessage("SendGroupMessage", text));
    }


    async void SendConnectionToserver(string method, string message)
    {
        try
        {
            await _connection.InvokeAsync(method, message);
        }
        catch (Exception ex)
        {
            Debug.Log("Error : " + ex.Message);
            return;
        }
    }


    async void sendMessage(string method,string userText)
    {
        try
        {
            await _connection.InvokeAsync(method, userText);
        }
        catch (Exception ex)
        {
            Debug.Log("Error : " + ex.Message);
            return;
        }
    }

    #region Post
    //Post 방식   
    IEnumerator postUnityWebRequest(UiManager uiMannager, string text)
    {
        WWWForm form = new WWWForm();

        form.AddField("UserName", "_aid");
        form.AddField("UserText", text);

        UnityWebRequest www = UnityWebRequest.Post("localhost:52460/Chat/", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            JsonData Objects = JsonMapper.ToObject(www.downloadHandler.text);
            uiMannager.SendMessageToChat(new Chat(Objects["userName"].ToString(), Objects["userText"].ToString()));
        }
    }

    #endregion
}