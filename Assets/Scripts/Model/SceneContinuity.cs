using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneContinuity : MonoBehaviour
{
    public AudioSource source;
    public AudioClip menuBGM;
    public AudioClip gameBGM;
    public float fadeTime;
    private bool startGame = true;

    private void Awake()
    {
        if(GameObject.Find("Audio Player") != null && GameObject.Find("Audio Player") != gameObject)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(transform.gameObject);
        SceneManager.activeSceneChanged += triggerFade;
    }

    private void triggerFade(Scene current, Scene next)
    {
        if(!startGame)
        {
            StartCoroutine(fade(0.5f, 0.01f, next.name));
        }
        else
        {
            startGame = false;
        }
    }

    private IEnumerator fade(float start, float end, string sceneName)
    {
        float timeElapsed = 0;
        while(timeElapsed < fadeTime)
        {
            source.volume = Mathf.Lerp(start, end, timeElapsed/fadeTime);
            timeElapsed += Time.deltaTime;
        }

        for(var timePassed = 0f; timePassed < fadeTime; timePassed += Time.deltaTime)
        {
            source.volume = Mathf.Lerp(start, end, timePassed / fadeTime);

            yield return null;
        }

        switch(sceneName)
        {
            case "MainMenu":
                source.clip = menuBGM;
                break;

            case "Cityv2":
                source.clip = gameBGM;
                break;
        }

        yield return new WaitForSeconds(0.1f);

        source.Play();
        
        for(var timePassed = 0f; timePassed < fadeTime; timePassed += Time.deltaTime)
        {
            source.volume = Mathf.Lerp(end, start, timePassed / fadeTime);

            yield return null;
        }
    }

    
}
