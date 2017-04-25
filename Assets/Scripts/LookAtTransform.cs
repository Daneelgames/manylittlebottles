using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTransform : MonoBehaviour {

    public Transform target;

	// Update is called once per frame
	void Update ()
    {
        Vector3 tempTarget = Vector3.Lerp(transform.position, target.position, 0.1f);
        transform.LookAt(tempTarget);
	}
}
