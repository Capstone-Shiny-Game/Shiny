using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyOrbitor : MonoBehaviour
{
    // Start is called before the first frame update
    private CinemachineOrbitalTransposer vcam;
    public float ySens = 0.05f;
    public float xSens = 1.0f;
    public float minY = 0.0f;
    public float maxY = 10.0f;
    private float startX;
    private float startY;
    private bool canRotate = true;
    void Start()
    {
        vcam = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineOrbitalTransposer>();
        startX = vcam.m_XAxis.Value;
        startY = vcam.m_FollowOffset.y;

    }

    public void ChangeAngle(float x, float y)
    {
        if (canRotate)
        {
            vcam.m_XAxis.Value += (xSens * x);
            vcam.m_FollowOffset.y = Mathf.Clamp(vcam.m_FollowOffset.y + y * ySens, minY, maxY);
        }
    }
    public void ResetZero()
    {
        if (this.isActiveAndEnabled && canRotate)
            StartCoroutine(Recenter(1f, 0f, 0f));
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
            vcam.m_FollowOffset.y = Mathf.SmoothStep(vcam.m_FollowOffset.y, destY, (elapsedTime / endTime));
            vcam.m_XAxis.Value = Mathf.SmoothStep(vcam.m_XAxis.Value, destX, (elapsedTime / endTime));
            canEnd = Mathf.Abs(vcam.m_XAxis.Value - destX) <= 4f && Mathf.Abs(vcam.m_FollowOffset.y - destY) <= 0.5f;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
       
        canRotate = true;


    }
}
