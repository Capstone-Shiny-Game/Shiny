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
            Destroy(gameObject);
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
    }

    private void dayEnd()
    {
        AkSoundEngine.PostEvent("night", gameObject);
    }

    private void triggerAudio(Scene current, Scene next)
    {
        if(current.buildIndex == 0)
        {
            AkSoundEngine.PostEvent("menu", gameObject);
        }
        else if(current.buildIndex == 1)
        {
            AkSoundEngine.PostEvent("levelStart", gameObject);
        }
        else
        {
            AkSoundEngine.PostEvent("credits", gameObject);
        }
    }    
}
