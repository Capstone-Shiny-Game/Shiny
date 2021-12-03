using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioAnim : MonoBehaviour
{
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void WingFlap()
    {
        audioSource.Play();
    }
}
