using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    private GameObject uiInventory;

    private void Start()
    {
        uiInventory = GameObject.Find("UI_inventory");
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f; // pause game time doesnt pass
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f; // set time back to normal
    }

    public void Home(int SceneID)
    {
        Time.timeScale = 1f; // set time back to normal
        SceneManager.LoadScene(SceneID);
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
