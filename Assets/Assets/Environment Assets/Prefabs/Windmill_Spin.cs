using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Windmill_Spin : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float speedMult;

  
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetFloat("SpinSpeed", speedMult);

        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetFloat("SpinSpeed", 1);

        }

    }


}
