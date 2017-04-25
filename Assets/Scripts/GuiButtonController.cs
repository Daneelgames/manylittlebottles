using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiButtonController : MonoBehaviour {

public JournalController journalController;

    public bool canBePressed = true;
    public bool selected = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canBePressed && selected)
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
                    journalController.anim.SetBool("Active", false);
                    StartCoroutine(SetButtonEnabled(journalController.buttonJournalBody, 0.5f));
                    canBePressed = false;
                    journalController.buttonCloseJournal.canBePressed = false;
                    GameManager.instance.StartCoroutine("Teleport");
                    break;
            }
        }
    }

    IEnumerator SetButtonEnabled (GuiButtonController button, float t)
    {
        yield return new WaitForSeconds (t);
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