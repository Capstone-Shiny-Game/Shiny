using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject controlsMenu;

    public void ChangeScene()
    {
        SceneManager.LoadScene("Cityv2");
    }

    public void enterControls()
    {
        mainMenu.SetActive(false);
        controlsMenu.SetActive(true);
    }

    public void exitControls()
    {
        mainMenu.SetActive(true);
        controlsMenu.SetActive(false);
    }
}
