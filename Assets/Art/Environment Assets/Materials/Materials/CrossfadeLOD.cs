using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossfadeLOD : MonoBehaviour
{
    [SerializeField] private float period;
    // Start is called before the first frame update
    void Start()
    {
        LODGroup.crossFadeAnimationDuration = period;
    }

   
}
