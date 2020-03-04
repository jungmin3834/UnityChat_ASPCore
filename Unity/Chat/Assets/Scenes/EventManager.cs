using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private UiManager _uiManager;

    private void Awake()
    {
        _uiManager = this.GetComponent<UiManager>();
    }

    public void SendTextMessage_Click()
    {
        _uiManager.Ui_SendMessage();
    }
}
