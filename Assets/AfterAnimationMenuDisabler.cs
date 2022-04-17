using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterAnimationMenuDisabler : MonoBehaviour
{
    public GameObject pauseMenuBackground;

    // Update is called once per frame
    void DisablePauseMenu()
    {
        pauseMenuBackground.SetActive(false);
    }
}
