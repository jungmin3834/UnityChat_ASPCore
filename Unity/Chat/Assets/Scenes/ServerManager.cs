using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;
using LitJson;

public class ChatViewModel
{
    public string UserName { get; set; }

    public string UserText { get; set; }
}

public class ServerManager : MonoBehaviour
{

    public static ServerManager instance;

    private void Start()
    {
        instance = this;
    }

    public void SendMessageToServer(UiManager uiMannager, string text)
    {
        StartCoroutine(postUnityWebRequest(uiMannager, text));
    }
    
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
            uiMannager.SendMessageToChat(Objects["userName"].ToString(), Objects["userText"].ToString());
        }
    }
}