using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleController : MonoBehaviour
{
    public Transform teleportPosition;
    public ParticleSystem teleportParticles;
    public GameObject cameraFocus;
    public Animator anim;
    public GameObject explosionParticles;
    public bool destroying = false;

    public GameObject bottleInterior;

    ParticleSystem.EmissionModule teleportParticlesEmission;

    void Start()
    {
        GameManager.instance.bottlesList.AddBottle(this);
        teleportParticlesEmission = teleportParticles.emission;
        if (GameManager.instance.playerShipController.parkingBottle != this)
            bottleInterior.SetActive(false);
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.name == "PlayerShip")
        {
            bottleInterior.SetActive(true);
        }
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
        destroying = true;
        anim.SetTrigger("Explode");
        yield return new WaitForSeconds(2f);
        Instantiate(explosionParticles, transform.position, transform.rotation);
        yield return new WaitForSeconds(0.25f);
        gameObject.SetActive(false);
    }
}