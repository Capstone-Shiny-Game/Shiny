using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public LevelLoader levelLoader;

    public void ChangeScene()
    {
        AkSoundEngine.PostEvent("none", gameObject);
        StartCoroutine(levelLoader.LoadLevel("Cityv2"));
    }

    public void credits()
    {   
        AkSoundEngine.PostEvent("buttonClick", gameObject);
        AkSoundEngine.PostEvent("none", gameObject);
        StartCoroutine(levelLoader.LoadLevel("credits"));
    }

    public void returnToMenu()
    {   
        AkSoundEngine.PostEvent("buttonClick", gameObject);
        AkSoundEngine.PostEvent("none", gameObject);
        StartCoroutine(levelLoader.LoadLevel("MainMenu"));
    }
}
