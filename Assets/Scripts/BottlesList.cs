using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottlesList : MonoBehaviour
{
    public List<BottleController> bottles;
    public void AddBottle(BottleController b)
    {
		bottles.Add(b);
    }
}
