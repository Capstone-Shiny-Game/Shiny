using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private GameObject uiInventory;

    private void Start()
    {
        uiInventory = GameObject.Find("UIInventory");
    }

    public void OnEnable()
    {
        Time.timeScale = 0f; // pause game time doesnt pass
        //TODO : fix this when opening the save menu
    }
    public void OnDisable()
    {
        Time.timeScale = 1f; // set time back to normal
        //TODO : fix this when opening the save menu
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // set time back to normal
        SceneManager.LoadScene("MainMenu");
    }

    public void ReloadScene()
    {
        Time.timeScale = 1f; // set time back to normal
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OpenInventory()
    {
        if (uiInventory is null)
        {
            Debug.Log("uiInventory is null in OpenInventory within PauseMenu.cs");
            return;
        }
        uiInventory.SetActive(true);
    }

    public void CloseInventory()
    {
        if (uiInventory is null)
        {
            Debug.Log("uiInventory is null in CloseInventory within PauseMenu.cs");
            return;
        }
        uiInventory.SetActive(false);
    }

    

}
