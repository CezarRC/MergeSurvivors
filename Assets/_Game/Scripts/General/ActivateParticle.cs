using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateParticle : MonoBehaviour
{
    ParticleSystem particle;
    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }
    private void OnEnable()
    {
        particle.Play();
    }
}
