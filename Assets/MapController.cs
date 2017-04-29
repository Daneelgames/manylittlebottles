using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    bool active = false;
    public void ToggleMap()
    {
        if (!active) // open map
        {
            StartCoroutine("OpenMap");
			InvokeRepeating("UpdateMap", 0.1f, 1f);
            active = true;
        }
        else //close map
        {
            StartCoroutine("CloseMap");
            active = false;
        }
    }

	void UpdateMap()
	{
		
	}

    IEnumerator OpenMap()
    {
        while (transform.localPosition.x < -1.01f)
        {
            print("open map");
            Vector3 newPos = new Vector3(-1, transform.localPosition.y, transform.localPosition.z);
            transform.localPosition = Vector3.Lerp(transform.localPosition, newPos, 0.2f);
            yield return null;
        }
        transform.localPosition = new Vector3(-1, transform.localPosition.y, transform.localPosition.z);
    }
    IEnumerator CloseMap()
    {
        while (transform.localPosition.x > -5.49f)
        {
            print("close map");
            Vector3 newPos = new Vector3(-5.5f, transform.localPosition.y, transform.localPosition.z);
            transform.localPosition = Vector3.Lerp(transform.localPosition, newPos, 0.2f);
            yield return null;
        }
        transform.localPosition = new Vector3(-5.5f, transform.localPosition.y, transform.localPosition.z);
    }
}