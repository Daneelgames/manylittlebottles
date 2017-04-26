using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementController : MonoBehaviour {

    public Vector3 posOffset;
    GameObject target;
    public float rotateSpeed = 5f;
    
    void Start()
    {
        target = GameManager.instance.playerShipController.parkingBottle.cameraFocus;
    }

    public void UpdateTarget(GameObject trgt)
    {
        target = trgt;
    }

    void LateUpdate()
    {
        //  turning cam
        if (Input.GetMouseButton(1))
        {
            // vertical
            posOffset = new Vector3(posOffset.x, posOffset.y + Input.GetAxis("Mouse Y") * 0.25f, posOffset.z); 
            if (posOffset.y > 1)
            {
                posOffset.y = 1;
            }
            else if (posOffset.y < -5)
            {
                posOffset.y = -5;
            }

            // horizontal
            float horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
            target.transform.Rotate(0, horizontal, 0);

            float desiredAngleY = target.transform.eulerAngles.y;
            float desiredAngleX = target.transform.eulerAngles.x;
            Quaternion rotation = Quaternion.Euler(0, desiredAngleY, 0);
            transform.position = target.transform.position - (rotation * posOffset);

            transform.LookAt(target.transform);
        }
    }
}