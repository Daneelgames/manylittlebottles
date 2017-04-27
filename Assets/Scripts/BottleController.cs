using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleController : MonoBehaviour
{
    public Transform teleportPosition;
    public ParticleSystem teleportParticles;
    public Transform bottleParent;
    public GameObject cameraFocus;
    public Animator anim;
    public GameObject explosionParticles;

    ParticleSystem.EmissionModule teleportParticlesEmission;

    void Start()
    {
        teleportParticlesEmission = teleportParticles.emission;
    }

    public void ShowParticles(bool active)
    {
        if (active)
        {
            if (GameManager.instance.control == GameManager.Control.Player)
            {
                // show particles at player pos
                teleportParticles.gameObject.transform.position = GameManager.instance.playerController.transform.position;
                teleportParticlesEmission.rateOverTime = 100;
            }
            else
            {
                // show particles at spawn point
                teleportParticles.gameObject.transform.position = teleportPosition.position;
                teleportParticlesEmission.rateOverTime = 100;
            }
        }
        else
        {
            teleportParticlesEmission.rateOverTime = 0;
        }
    }

    public IEnumerator Explode()
    {
        anim.SetTrigger("Explode");
        yield return new WaitForSeconds(2f);
        /// explode, ship flyes away
        yield return new WaitForSeconds(0.25f);
        Instantiate(explosionParticles, transform.position, transform.rotation);
        gameObject.SetActive(false);
    }
}