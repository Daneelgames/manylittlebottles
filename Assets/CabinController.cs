using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabinController : MonoBehaviour
{
    public bool cabinIsActive = false;
    public Animator anim;
    public void ShowCabin(bool active)
    {
        if (active)
        {
            anim.SetBool("Active", true);
        }
        else
        {
            anim.SetBool("Active", false);
        }
        cabinIsActive = active;
    }

    public IEnumerator Shake(float t)
    {
        anim.SetBool("Shake", true);
        yield return new WaitForSeconds(t);
        anim.SetBool("Shake", false);
    }
}