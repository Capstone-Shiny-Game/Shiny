using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTrialRing : MonoBehaviour
{
    public delegate void RingCollision(GameObject ring);
    public static event RingCollision OnRingCollision;

    private ParticleSystem popinParticles;

    private void Start()
    {
        popinParticles = GetComponentInChildren<ParticleSystem>();
    }

    private void OnEnable()
    {
        popinParticles.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        OnRingCollision?.Invoke(gameObject);
    }
}
