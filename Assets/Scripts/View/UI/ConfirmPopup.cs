using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmPopup : MonoBehaviour
{
    public delegate void ConfirmHander(bool e);
    private event ConfirmHander BroadcastConfirm;
    private ConfirmHander listener;
    [HideInInspector] public string message;
    public TMPro.TMP_Text messageBox;
    public TMPro.TMP_Text confirmButtonText;
    public TMPro.TMP_Text cancelButtonText;
    void OnEnable()
    {
        if ((message is null) || (message == ""))
        {
            message = "error in ConfirmPopup.cs message not set to a value";
        }
        messageBox.SetText(message);
    }
    /// <summary>
    /// shows the pupup with the passed in message and calls the passed in handler with the user choice
    /// </summary>
    /// <param name="message"></param>
    /// <param name="handler"></param>
    public void ShowPopUP(string message, ConfirmHander handler, string confirmText = "confirm", string cancelText = "cancel")
    {
        this.message = message;
        confirmButtonText.text = confirmText;
        cancelButtonText.text = cancelText;
        SetListener(handler);
        gameObject.SetActive(true);
    }

    public void ConfirmClicked(bool value)
    {
        BroadcastConfirm?.Invoke(value);
    }

    public void SetListener(ConfirmHander handler)
    {
        BroadcastConfirm -= listener;
        BroadcastConfirm += handler;
        listener = handler;
    }
}
