using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowSplashArt : MonoBehaviour
{

    [Serializable]
    public struct SpritePair
    {
        public PatioUtility.Toys tName;
        public string subtitle;
        public Sprite spr;
    }

    public SpritePair[] images;
    public GameObject panel;
    public GameObject panelSpr;
    private TMP_Text subText;
    public void Start()
    {
        subText = panel.GetComponentInChildren<TMP_Text>();
        panel.SetActive(false);
    }
    public void SetImage(PatioUtility.Toys type)
    {
        AkSoundEngine.PostEvent("buttonClick", gameObject);
        panel.SetActive(true);
        foreach (SpritePair i in images)
        {
            if (i.tName == type)
            {
                panelSpr.GetComponent<Image>().sprite = i.spr;
                subText.text = i.subtitle;
            }

        }
    }

    public void SetImage(string type)
    {
        AkSoundEngine.PostEvent("buttonClick", gameObject);
        panel.SetActive(true);
        foreach (SpritePair i in images)
        {
            if (PatioUtility.GetPrettyName(i.tName) == type)
            {
                panelSpr.GetComponent<Image>().sprite = i.spr;
                subText.text = i.subtitle;

            }

        }
    }
    public void CloseImage()
    {
        AkSoundEngine.PostEvent("buttonClick", gameObject);
        panel.SetActive(false);
    }

}
