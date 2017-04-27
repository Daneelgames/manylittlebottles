using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipController : MonoBehaviour
{

    public BottleController parkingBottle; // ship sits on this bottle
    public GameObject cameraFocus;
    public Animator anim;
    public ParticleSystem taleParticles;
    ParticleSystem.EmissionModule _taleParticles;
    void Start()
    {
        _taleParticles = taleParticles.emission;
    }
    public IEnumerator TakeOff()
    {
        _taleParticles.rateOverTime = 100;
        GameManager.instance.cabinController.StartCoroutine("Shake", 2f);
        parkingBottle.StartCoroutine("Explode");
        yield return new WaitForSeconds(2f);
        parkingBottle = null;
        transform.SetParent(null);
    }
}