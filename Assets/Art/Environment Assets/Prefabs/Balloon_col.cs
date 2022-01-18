using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon_col : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("Balloon_Col", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("Balloon_Col", false);
        }
    }
}
