using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneContinuity : MonoBehaviour
{
    private void Start()
    {
        if(GameObject.Find("Audio Player") != null && GameObject.Find("Audio Player") != gameObject)
        {
            DestroyImmediate(gameObject);
            return;
        }

        DontDestroyOnLoad(transform.gameObject);
        SceneManager.activeSceneChanged += triggerAudio;
        AkSoundEngine.PostEvent("menuStart", gameObject);
        DayController.OnMorningEvent += dayStart;
        DayController.OnEveningEvent += dayEnd;
    }

    private void dayStart()
    {
        AkSoundEngine.PostEvent("dayStart", gameObject);

        if(SceneManager.GetActiveScene().name == "MainMenu")
        {
            return;
        }

        AkSoundEngine.PostEvent("none", gameObject);
    }

    private void dayEnd()
    {
        if(SceneManager.GetActiveScene().name == "MainMenu")
        {
            return;
        }

        AkSoundEngine.PostEvent("night", gameObject);
    }

    private void triggerAudio(Scene current, Scene next)
    {
        if(next.buildIndex == 0)
        {
            AkSoundEngine.PostEvent("menu", gameObject);
        }
        else if(next.buildIndex == 1)
        {
            AkSoundEngine.PostEvent("levelStart", gameObject);
        }
        else if(next.buildIndex == 2)
        {
            AkSoundEngine.PostEvent("credits", gameObject);
        }
    }    
}
