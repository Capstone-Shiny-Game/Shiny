using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatUpDown : MonoBehaviour
{
    public float Speed;
    public float movementRange;
    private float startingHeight;
    public bool enableWaitRandomDelay = false;
    [Tooltip("max time in seconds to wait"), Range(2, 31)]
    public int maxWait = 4;
    private float mytime;
    System.Random randomNumGenerator;
    private void Start()
    {
        startingHeight = gameObject.transform.localPosition.y;
        mytime = 0;
        randomNumGenerator = new System.Random();
    }
    void Update()
    {
        if (!enableWaitRandomDelay)
        {
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, startingHeight + UpDown(), gameObject.transform.localPosition.z);
        }
        else {
            StartCoroutine(UpDownWaitCoroutine());
        }
    }
    /// <summary>
    /// uses the sine function to determine the new hight of the object.
    /// </summary>
    /// <returns></returns>
    public float UpDown()
    {
            return movementRange * Mathf.Sin(Speed * (Time.time));
    }



    IEnumerator UpDownWaitCoroutine()
    {
        float upDown = Mathf.Sin(Speed * (mytime));
        if (upDown < -.95 || upDown > .95)
        {
            int randNum = randomNumGenerator.Next(1, maxWait);
            //yield on a new YieldInstruction that waits for random seconds.
            yield return new WaitForSeconds(randNum);
        }
        mytime += Time.deltaTime;
        upDown = Mathf.Sin(Speed * (mytime));
        upDown = movementRange * upDown;
        Debug.Log("" + mytime + " , " + upDown);
        gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, startingHeight + upDown, gameObject.transform.localPosition.z);
    }
}
