using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;

public class ServerManager : MonoBehaviour
{

    //The URL to the server - In our case localhost with port number 2475
    private string url = @"https://localhost:44359/HelloWorld/welcome";

    private void Start()
    {
        StartCoroutine(postUnityWebRequest());
    }


    IEnumerator postUnityWebRequest()
    {
        string url = "https://localhost:44359/HelloWorld/welcome";

        WWW www = new WWW(url);
        while (!www.isDone)
            yield return null;

        if (string.IsNullOrEmpty(www.error))
        {
            Debug.Log(www.text);
        }
        else
            Debug.Log(www.error);
    }
}