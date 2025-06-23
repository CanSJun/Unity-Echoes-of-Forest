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
        // 파티클 재생 시간과 수명을 더해준다
    }
}
