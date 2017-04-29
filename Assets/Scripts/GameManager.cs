using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public bool haveJournal = false;

    public enum Control { Player, Ship };
    public Control control = Control.Player;

    public SimpleCharacterControl playerController;
    public PlayerShipController playerShipController;
    public JournalController journalController;
    public CabinController cabinController;

    public List<InteractiveObject> objectsToInteract;
    public CameraMovementController cameraMovementController;
    public BottlesList bottlesList;

    void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (haveJournal)
            journalController.EnableJournal();

        StartPositions();
    }
    void StartPositions()
    {
        playerController.transform.parent = playerShipController.parkingBottle.bottleInterior.transform;
        playerController.transform.position = playerShipController.parkingBottle.teleportPosition.position;
        playerShipController.transform.parent = playerShipController.parkingBottle.bottleInterior.transform;

    }

    public IEnumerator Teleport()
    {
        // special effects here
        playerShipController.parkingBottle.ShowParticles(true);
        yield return new WaitForSecondsRealtime(0.5f);
        // and here
        switch (control)
        {
            case Control.Player: // go to ship
                playerController.gameObject.SetActive(false);
                playerController.gameObject.transform.SetParent(playerShipController.gameObject.transform);
                playerController.gameObject.transform.localPosition = Vector3.zero;
                control = Control.Ship;
                cameraMovementController.UpdateTarget(playerShipController.cameraFocus);
                cabinController.ShowCabin(true);
                break;

            case Control.Ship: // go to bottle
                playerController.gameObject.transform.SetParent(playerShipController.parkingBottle.bottleInterior.transform);
                playerController.gameObject.transform.position = playerShipController.parkingBottle.teleportPosition.position;
                playerController.gameObject.transform.rotation = playerShipController.parkingBottle.teleportPosition.rotation;
                playerController.gameObject.SetActive(true);
                control = Control.Player;
                cameraMovementController.UpdateTarget(playerShipController.parkingBottle.cameraFocus);
                cabinController.ShowCabin(false);
                break;
        }
        playerShipController.parkingBottle.ShowParticles(false);
    }

    public void SetObjectTointeract(InteractiveObject obj, bool enter) // enter is true if object enters the collider
    {
        if (enter)
        {
            objectsToInteract.Add(obj);
        }
        if (!enter)
        {
            foreach (InteractiveObject j in objectsToInteract)
            {
                if (j == obj)
                {
                    objectsToInteract[objectsToInteract.IndexOf(j)].SetGuiAnimatorBool("Active", false);
                    objectsToInteract.Remove(j);
                    break;
                }
            }
        }
        if (objectsToInteract.Count > 0)
        {
            objectsToInteract[0].SetGuiAnimatorBool("Active", true);
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Action") && objectsToInteract.Count > 0 && objectsToInteract[0].canInteract)
        {
            objectsToInteract[0].StartCoroutine("SetInactiveOverTimer", 0.5f);
            if (objectsToInteract[0].name == "JournalDrop")
            {
                // give visual feedback
                haveJournal = true;
                journalController.anim.SetBool("Enabled", true);
                objectsToInteract.RemoveAt(0);
            }
        }
    }
}