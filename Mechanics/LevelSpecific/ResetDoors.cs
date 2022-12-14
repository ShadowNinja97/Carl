using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetDoors : MonoBehaviour
{


    public DoorToggleButton[] dtbs;

    private Animator anim;

    public void ResetDoor()
    {
        for (int i = 0; i < dtbs.Length; i++)
            dtbs[i].CloseDoor();
    }
}
