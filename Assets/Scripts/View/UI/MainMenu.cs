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
        StartCoroutine(levelLoader.LoadLevel("Cityv2"));
    }
}
