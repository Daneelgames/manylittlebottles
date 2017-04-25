using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour {

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
    }
}