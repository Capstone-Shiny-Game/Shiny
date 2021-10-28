using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Windmill_Spin : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float speedMult;
    float lerpDuration = 2;




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
            StartCoroutine(Lerp());
           
        }

    }

    IEnumerator Lerp()
    {
        float timeElapsed = 0;

        while (timeElapsed < lerpDuration)
        {
            animator.SetFloat("SpinSpeed", Mathf.Lerp(speedMult, 1, timeElapsed / lerpDuration));
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        animator.SetFloat("SpinSpeed", 1);
    }

}
