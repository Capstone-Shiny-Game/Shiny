using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmPopup : MonoBehaviour
{
    public delegate void ConfirmHander(bool e);
    private event ConfirmHander BroadcastConfirm;
    private ConfirmHander listener;
    [HideInInspector]public string message;
    public TMPro.TMP_Text messageBox;
    void Start()
    {
        if ((message is null) || (message == "")) {
            message = "error in ConfirmPopup.cs message not set to a value";
        }
        messageBox.SetText(message);
    }

    public void ConfirmClicked(bool value) {
        BroadcastConfirm?.Invoke(value);
    }

    public void SetListener(ConfirmHander handler)
    {
        BroadcastConfirm -= listener;
        BroadcastConfirm += handler;
        listener = handler;
    }
}
