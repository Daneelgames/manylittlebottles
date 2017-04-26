using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveObject : MonoBehaviour {

    public Animator guiAnimator;
    public Animator anim;
    public bool canInteract = true;


    void OnTriggerEnter (Collider coll)
    {
        if (coll.tag == "Player" && canInteract)
        {
            GameManager.instance.SetObjectTointeract(this, true);
        }
    }

    void OnTriggerExit(Collider coll)
    {
        if (coll.tag == "Player")
        {
            GameManager.instance.SetObjectTointeract(this, false);
        }
    }

    public void SetGuiAnimatorBool (string boolname, bool active)
    {
        guiAnimator.SetBool(boolname, active);
    }

    public IEnumerator SetInactiveOverTimer(float t)
    {
        SetGuiAnimatorBool("Active", false);
        anim.SetBool("Active", false);
        canInteract = false;
        yield return new WaitForSeconds(t);
        gameObject.SetActive(false);
    }
}