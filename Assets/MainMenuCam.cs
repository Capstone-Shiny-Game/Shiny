using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MainMenuCam : MonoBehaviour
{
    public float speed;
    private CinemachineVirtualCamera cam;
    void Start()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
        StartCoroutine(anim());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator anim()
    {
        float timeElapsed = 0;
        while (timeElapsed < speed)
        {
            cam.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition = Mathf.Lerp(0, 1, timeElapsed/speed);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(anim());
    }
}
