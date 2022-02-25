using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatUpDown : MonoBehaviour
{
    public float Speed;
    public float movementRange;
    void Update()
    {
        gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y + UpDown(), gameObject.transform.localPosition.z);
    }
    /// <summary>
    /// uses the sine function to determine the new hight of the object.
    /// </summary>
    /// <returns></returns>
    public float UpDown()
    {
        return movementRange * Mathf.Sin(Speed * (Time.time));
    }
}
