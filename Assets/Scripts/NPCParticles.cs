using System;
using System.Collections;
using UnityEngine;

public class NPCParticle : MonoBehaviour
{
    [SerializeField] 
    private ParticleSystem deathParticlePrefab;

    private void Start()
    {
        GetComponent<IHealth>().OnDied += HandleNPCDied;
    }

    private void HandleNPCDied()
    {
        var deathparticle = Instantiate(deathParticlePrefab, transform.position, transform.rotation) ;
        Destroy(deathparticle, 4f);
        Destroy(this.gameObject);
    }
}