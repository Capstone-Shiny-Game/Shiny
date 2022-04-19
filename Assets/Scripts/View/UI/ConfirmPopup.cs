using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmPopup : MonoBehaviour
{
    public delegate void ConfirmHander(bool e);
    private event ConfirmHander broadcastConfirm;
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
        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<InputController>().menuOpen = true;
        Time.timeScale = 0f;
        this.message = message;
        confirmButtonText.text = confirmText;
        cancelButtonText.text = cancelText;
        SetListener(handler);
        gameObject.SetActive(true);
    }

    public void ConfirmClicked(bool value)
    {
        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<InputController>().menuOpen = false;
        Time.timeScale = 1f;
        AkSoundEngine.PostEvent("buttonClick", gameObject);
        broadcastConfirm?.Invoke(value);
    }

    public void SetListener(ConfirmHander handler)
    {
        broadcastConfirm -= listener;
        broadcastConfirm += handler;
        listener = handler;
    }
}
