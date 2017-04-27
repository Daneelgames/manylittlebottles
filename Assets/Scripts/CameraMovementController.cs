using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementController : MonoBehaviour {

    public Vector3 posOffset;
    GameObject target;
    public float rotateSpeed = 5f;
    float mouseX = 0;

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
            mouseX = Input.GetAxis("Mouse X");
            if (mouseX > 0.5f)
            {
                mouseX = 0.5f;
            }
            else if (mouseX < -0.5f)
            {
                mouseX = -0.5f;
            }
            float horizontal = mouseX * rotateSpeed;
            target.transform.Rotate(0, horizontal, 0);

            //transform.LookAt(target.transform);
        }
        float desiredAngleY = target.transform.eulerAngles.y;
        float desiredAngleX = target.transform.eulerAngles.x;
        Quaternion rotation = Quaternion.Euler(0, desiredAngleY, 0);
        transform.position = Vector3.Lerp(transform.position, target.transform.position - (rotation * posOffset), 7 * Time.deltaTime);

        var targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 7 * Time.deltaTime);

    }
}