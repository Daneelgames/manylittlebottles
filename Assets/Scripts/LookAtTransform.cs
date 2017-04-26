using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTransform : MonoBehaviour {

    public Transform target;

	void Update ()
    {
        //Vector3 tempTarget = Vector3.Lerp(transform.position, target.position, 0.1f);
        //transform.LookAt(tempTarget);

        var targetRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5 * Time.deltaTime);
    }
}
