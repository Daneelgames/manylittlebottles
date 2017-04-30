using System.Collections;
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
        mouseInitPos.z = 10f;
        Vector3 temp = Camera.main.ScreenToWorldPoint(mouseInitPos);
        mouseInitPos = temp;

        while (Input.GetMouseButton(0))
        {
            Vector3 mouseOffset = Input.mousePosition;
            mouseOffset.z = 10f;
            Vector3 offsetTemp = Camera.main.ScreenToWorldPoint(mouseOffset);
            mouseOffset = offsetTemp;

            Vector2 joustickVelocity = Vector2.zero;
            if (mouseOffset.x < mouseInitPos.x)
            {
                float offset = Mathf.Abs(Mathf.Abs(mouseInitPos.x) - Mathf.Abs(mouseOffset.x));
                if (offset <= 1) joustickVelocity.x = offset * -1;
                else joustickVelocity.x = -1;
            }
            else if (mouseOffset.x > mouseInitPos.x)
            {
                float offset = Mathf.Abs(mouseOffset.x) - Mathf.Abs(Mathf.Abs(mouseInitPos.x));
                if (offset <= 1) joustickVelocity.x = offset;
                else joustickVelocity.x = 1;
            }
            if (mouseOffset.y < mouseInitPos.y)
            {
                float offset = Mathf.Abs(Mathf.Abs(mouseInitPos.y) - Mathf.Abs(mouseOffset.y)) * 2;
                if (offset <= 1) joustickVelocity.y = offset * -1;
                else joustickVelocity.y = -1;
            }
            else if (mouseOffset.y > mouseInitPos.y)
            {
                float offset = Mathf.Abs(Mathf.Abs(mouseOffset.y) - Mathf.Abs(mouseInitPos.y)) * 2;
                if (offset <= 1) joustickVelocity.y = offset;
                else joustickVelocity.y = 1;
            }
            TiltJoystick(joustickVelocity);
            if (!GameManager.instance.playerShipController.parkingBottle)
                GameManager.instance.playerShipController.TiltShip(joustickVelocity);
            print(joustickVelocity);
            yield return null;
        }
    }

    void TiltJoystick(Vector2 joystickVelocity)
    {
        // if (x == 1) rotationZ = -35
        // if (y == 1) rotationX = 35
        Quaternion newRotation = transform.rotation;
        //        Vector3 targetRotation =  new Vector3(35 / joystickVelocity.y, 0, 35/joystickVelocity.x);
        Quaternion targetRotation = transform.rotation;
        float z = 35 * joystickVelocity.x;
        float x = 35 * joystickVelocity.y;
        targetRotation.eulerAngles = new Vector3(z, 0, x);
        //newRotation.eulerAngles = Vector3.Lerp(newRotation.eulerAngles, targetRotation, 0.8f);
        transform.rotation = targetRotation;
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