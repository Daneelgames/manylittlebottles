using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour {

    float startMouseX = 0;

	// Use this for initialization
	void Start () {
        Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 temp = Input.mousePosition;
        temp.z = 10f; 
        this.transform.position = Camera.main.ScreenToWorldPoint(temp);
        //print(Input.mousePosition);

        //turn bottle

        /*
        if (GameManager.instance.control == GameManager.Control.Player)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 temp_2 = Input.mousePosition;
                temp_2.z = 10f;
                startMouseX = Camera.main.ScreenToWorldPoint(temp_2).x;
            }
            if (Input.GetMouseButton(0))
            {
                Transform newRotation = GameManager.instance.playerShipController.parkingBottle.transform;
                newRotation.Rotate(Vector3.up, Input.GetAxis("Mouse X") * -5f);
                GameManager.instance.playerShipController.parkingBottle.transform.rotation = Quaternion.Lerp(GameManager.instance.playerShipController.parkingBottle.transform.rotation, newRotation.rotation, 100f);
                
        //////////////////////////
                Vector3 temp_3 = Input.mousePosition;
                temp_3.z = 10f;
                float newMouseX = Camera.main.ScreenToWorldPoint(temp_3).x;
                if (startMouseX > newMouseX)
                {
                    print(" rotate to left; axis is " + Input.GetAxis("Mouse X"));
                    GameManager.instance.playerShipController.parkingBottle.transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * 2);
                }
                else
                {
                    print(" rotate to right; axis is " + Input.GetAxis("Mouse X"));
                    GameManager.instance.playerShipController.parkingBottle.transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * 2);
                }
            }
        }
        */
    }
}