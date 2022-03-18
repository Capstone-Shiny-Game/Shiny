using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyOrbitor : MonoBehaviour
{
    // Start is called before the first frame update
    private CinemachineOrbitalTransposer vcam;
    public float ySens =5f;
    public float xSens = 5f;
    public float minY = 0.0f;
    public float maxY = 10.0f;
    private float startX;
    private float startY;
    private bool canRotate = true;
    private float moveX =0.0f;
    private float moveY =0.0f;
    void Start()
    {
        vcam = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineOrbitalTransposer>();
        startX = vcam.m_XAxis.Value;
        startY = vcam.m_FollowOffset.y;

    }
    void Update()
    {
        if (this.isActiveAndEnabled && canRotate)
        {
            vcam.m_XAxis.Value += (xSens * moveX);
            vcam.m_FollowOffset.y = Mathf.Clamp(vcam.m_FollowOffset.y + moveY * ySens, minY, maxY);
        }
    }
    public void ChangeAngle(float x, float y)
    {
        moveX = x;
        moveY = y;
    }

    
    public void ResetZero()
    {
        if (this.isActiveAndEnabled && canRotate)
            StartCoroutine(Recenter(.5f, 0f, 0f));
    }
    public void Reset()
    {
        if (this.isActiveAndEnabled && canRotate)
            StartCoroutine(Recenter(5f,startX,startY));
    }
    public IEnumerator Recenter(float endTime, float destX, float destY)
    {
        canRotate = false;

        float elapsedTime = 0;
        bool canEnd = false;
        while (elapsedTime < endTime && !canEnd)
        {
            if (vcam == null)
                break;
            vcam.m_FollowOffset.y = Mathf.SmoothStep(vcam.m_FollowOffset.y, destY, (elapsedTime / endTime));
            vcam.m_XAxis.Value = Mathf.SmoothStep(vcam.m_XAxis.Value, destX, (elapsedTime / endTime));
            canEnd = Mathf.Abs(vcam.m_XAxis.Value - destX) <= 4f && Mathf.Abs(vcam.m_FollowOffset.y - destY) <= 0.5f;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
       
        canRotate = true;


    }
}
