using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShipController : MonoBehaviour
{
    public enum Control { TakeOff, Landing, Auto, Manual, Idle };
    public Control control = Control.Idle;
    public BottleController parkingBottle; // ship sits on this bottle
    public GameObject cameraFocus;
    public Animator anim;
    public ParticleSystem taleParticles;
    public float flySpeedTakeOff = 5;
    public float flySpeedIdle = 0.1f;
    float flySpeed = 5;
    ParticleSystem.EmissionModule _taleParticles;
    public Rigidbody rb;
    public float turnSpeed;
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
        GameManager.instance.cabinController.ShakeBig();
        flySpeed = flySpeedTakeOff;
        control = Control.TakeOff; // ship fly up
        anim.SetBool("Rotate", true);
        parkingBottle = null;
        transform.SetParent(null);

        while (transform.rotation.eulerAngles.x < 355)
        {
            var newRotation = new Quaternion();
            newRotation.eulerAngles = Vector3.zero;
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, 1f * Time.deltaTime);
            transform.Translate(Vector3.forward * flySpeed * Time.deltaTime, Space.Self);
            yield return null;
        }
        _taleParticles.rateOverTime = 0;
        control = Control.Idle;
        StartCoroutine("FixIdleRotation");
        StartCoroutine("FixIdleSpeed");
        StartCoroutine("IdleInSpace");
    }

    IEnumerator FixIdleRotation()
    {
        while (transform.rotation.eulerAngles.x > 0.1 && transform.rotation.eulerAngles.x < 359.9)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(Vector3.zero), 1f * Time.deltaTime);
            yield return null;
        }
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }
    IEnumerator FixIdleSpeed()
    {
        while (flySpeed > 0.11)
        {
            flySpeed = Mathf.Lerp(flySpeed, flySpeedIdle, 0.01f);
            yield return null;
        }
        flySpeed = 0.1f;
    }

    IEnumerator IdleInSpace()
    {
        while (control == Control.Idle && !parkingBottle)
        {
            transform.Translate(Vector3.forward * flySpeed * Time.deltaTime, Space.Self);
            yield return null;
        }
    }
    public void Maneuvering(Vector2 joystickVelocity)
    {
        float newx = Mathf.Lerp(rb.angularVelocity.x, joystickVelocity.y * turnSpeed, 0.9f);
        float newy = Mathf.Lerp(rb.angularVelocity.y, joystickVelocity.x * turnSpeed, 0.9f);

        float xxx = transform.localEulerAngles.x;

        if (xxx > 25 && xxx < 300 && joystickVelocity.y > 0)
        {
            print(xxx);
            newx = 0;
        }
        else if ((xxx < 335 && xxx > 300 && joystickVelocity.y < 0))
        {
            print(xxx);
            newx = 0;
        }

        rb.angularVelocity = new Vector3(0, newy, 0);
    }
}