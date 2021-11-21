using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;//pause game time doesnt pass
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;//set time back to normal

    }

    public void Home(int SceneID) {
        Time.timeScale = 1f;//set time back to normal
        SceneManager.LoadScene(SceneID);

    }

}
