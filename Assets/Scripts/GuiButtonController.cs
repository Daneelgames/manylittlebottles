﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiButtonController : MonoBehaviour
{

    public JournalController journalController;

    public bool canBePressed = true;
    public bool selected = false;
    public Animator anim;

    float cooldown = 0.5f;

    void Update()
    {
        if (cooldown > 0)
            cooldown -= Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && canBePressed && selected && cooldown <= 0f)
        {
            switch (name)
            {
                case "JournalBody": // open journal
                    journalController.anim.SetBool("Active", true);
                    StartCoroutine(SetButtonEnabled(journalController.buttonCloseJournal, 0.5f));
                    StartCoroutine(SetButtonEnabled(journalController.buttonTransfer, 0.5f));
                    canBePressed = false;
                    break;

                case "CloseJournal": // close journal
                    journalController.anim.SetBool("Active", false);
                    StartCoroutine(SetButtonEnabled(journalController.buttonJournalBody, 0.5f));
                    canBePressed = false;
                    journalController.buttonTransfer.canBePressed = false;
                    break;

                case "Transfer": // go in and out the ship
                    bool canTransfer = true;

                    if (GameManager.instance.playerShipController.parkingBottle && GameManager.instance.playerShipController.parkingBottle.destroying)
                    {
                        canTransfer = false;
                    }
                    else if (GameManager.instance.control == GameManager.Control.Ship && !GameManager.instance.playerShipController.parkingBottle)
                    {
                        canTransfer = false;
                    }

                    if (canTransfer)
                    {
                        journalController.anim.SetBool("Active", false);
                        StartCoroutine(SetButtonEnabled(journalController.buttonJournalBody, 0.5f));
                        canBePressed = false;
                        journalController.buttonCloseJournal.canBePressed = false;
                        GameManager.instance.StartCoroutine("Teleport");
                    }
                    break;

                case "TakeOff": // fly up, destroy bottle
                    if (GameManager.instance.playerShipController.parkingBottle && !GameManager.instance.playerShipController.parkingBottle.destroying)
                        GameManager.instance.playerShipController.StartCoroutine("TakeOff");
                    break;

                case "Joystick":
                    StartCoroutine("JoystickControl");
                    break;
            }
            if (anim)
                anim.SetTrigger("Press");
            cooldown = 0.5f;
        }
    }

    IEnumerator JoystickControl()
    {
        Vector3 mouseInitPos = Input.mousePosition;
        //mouseInitPos.z = 10f;
        //Vector3 temp = Camera.main.ScreenToWorldPoint(mouseInitPos);
        //mouseInitPos = temp;

        while (Input.GetMouseButton(0))
        {
            Vector3 mouseOffset = Input.mousePosition;
            //mouseOffset.z = 10f;
            //Vector3 offsetTemp = Camera.main.ScreenToWorldPoint(mouseOffset);
            //mouseOffset = offsetTemp;

            Vector2 joustickVelocity = Vector2.zero;

            float maxOffsetX = Screen.width / 10;
            float maxOffsetY = Screen.height / 10;

            if (mouseOffset.x < mouseInitPos.x)
            {
                float offset = Mathf.Abs(Mathf.Abs(mouseInitPos.x) - Mathf.Abs(mouseOffset.x));
                if (offset <= maxOffsetX) joustickVelocity.x = (offset / maxOffsetX) * -1;
                else joustickVelocity.x = -1;
            }
            else if (mouseOffset.x > mouseInitPos.x)
            {
                float offset = Mathf.Abs(mouseOffset.x) - Mathf.Abs(Mathf.Abs(mouseInitPos.x));
                if (offset <= maxOffsetX) joustickVelocity.x = offset / maxOffsetX;
                else joustickVelocity.x = 1;
            }
            if (mouseOffset.y < mouseInitPos.y)
            {
                float offset = Mathf.Abs(Mathf.Abs(mouseInitPos.y) - Mathf.Abs(mouseOffset.y));
                if (offset <= maxOffsetY) joustickVelocity.y = (offset / maxOffsetY) * -1;
                else joustickVelocity.y = -1;
            }
            else if (mouseOffset.y > mouseInitPos.y)
            {
                float offset = Mathf.Abs(Mathf.Abs(mouseOffset.y) - Mathf.Abs(mouseInitPos.y));
                if (offset <= maxOffsetY) joustickVelocity.y = offset / maxOffsetY;
                else joustickVelocity.y = 1;
            }
            TiltJoystick(joustickVelocity);
            if (!GameManager.instance.playerShipController.parkingBottle)
                GameManager.instance.playerShipController.Maneuvering(joustickVelocity);
            //            print(joustickVelocity);
            yield return null;
        }
        StartCoroutine("ReturnJoystick");
    }

    void TiltJoystick(Vector2 joystickVelocity)
    {
        // if (x == 1) rotationZ = -35
        // if (y == 1) rotationX = 35
        //        Vector3 targetRotation =  new Vector3(35 / joystickVelocity.y, 0, 35/joystickVelocity.x);
        Quaternion targetRotation = transform.rotation;
        float x = 35 * joystickVelocity.y;
        float z = 35 * joystickVelocity.x * -1;
        targetRotation.eulerAngles = new Vector3(x, 0, z);
        float newX = Mathf.Lerp(transform.localEulerAngles.x, targetRotation.eulerAngles.x, 0.9f);
        float newZ = Mathf.Lerp(transform.localEulerAngles.z, targetRotation.eulerAngles.z, 0.9f);
        //transform.localEulerAngles = new Vector3(newX, 0, newZ);
        transform.localEulerAngles = targetRotation.eulerAngles;
        //print (targetRotation.eulerAngles);
    }

    IEnumerator ReturnJoystick()
    {
        float timer = 0f;
        Quaternion startRot = transform.rotation;
        while (timer <= 1f)
        {
            timer += Time.deltaTime * 20;
            transform.localRotation = Quaternion.Lerp(startRot, Quaternion.identity, timer);
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 0, transform.localEulerAngles.z);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator SetButtonEnabled(GuiButtonController button, float t)
    {
        yield return new WaitForSeconds(t);
        button.canBePressed = true;
    }

    void OnMouseOver()
    {
        selected = true;
        //print("mouse entered " + name);

        switch (name)
        {
            case "JournalBody":
                if (canBePressed)
                    journalController.anim.SetBool("Selected", true);
                break;
        }
    }

    void OnMouseExit()
    {
        selected = false;

        switch (name)
        {
            case "JournalBody":
                journalController.anim.SetBool("Selected", false);
                break;
        }
    }
}