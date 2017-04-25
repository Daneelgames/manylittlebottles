using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance = null;

    public enum Control { Player, Ship };
    public Control control = Control.Player;

    public SimpleCharacterControl playerController;
    public PlayerShipController playerShipController;

    void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public IEnumerator Teleport()
    {
        // special effects here
        yield return new WaitForSecondsRealtime(0.5f);
        // and here
        switch (control)
        {
            case Control.Player: // go to ship
                playerController.gameObject.SetActive(false);
                playerController.gameObject.transform.SetParent(playerShipController.gameObject.transform);
                playerController.gameObject.transform.localPosition = Vector3.zero;
                control = Control.Ship;
                break;

            case Control.Ship: // go to bottle
                playerController.gameObject.transform.SetParent(playerShipController.parkingBottle.transform);
                playerController.gameObject.transform.position = playerShipController.parkingBottle.teleportPosition.position;
                playerController.gameObject.transform.rotation = playerShipController.parkingBottle.teleportPosition.rotation;
                playerController.gameObject.SetActive(true);
                control = Control.Player;
                break;
        }
    }
}