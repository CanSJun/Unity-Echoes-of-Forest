using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle_System : MonoBehaviour
{

    private ParticleSystem particle;
    void Start()
    {
        particle = GetComponent<ParticleSystem>();
        particle.Play();
        Destroy(gameObject, particle.main.duration + particle.main.startLifetime.constantMax);
        // ��ƼŬ ��� �ð��� ������ �����ش�
    }
}
