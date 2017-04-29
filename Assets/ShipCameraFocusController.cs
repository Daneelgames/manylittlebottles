using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCameraFocusController : MonoBehaviour
{

    void Start()
    {
        transform.parent = null;
    }

    void Update()
    {
        if (GameManager.instance.control == GameManager.Control.Ship)
            transform.position = GameManager.instance.playerShipController.transform.position;
    }
}
