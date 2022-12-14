using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCameraMove : Interactable
{
    public CameraWaypointMover camWaypoint;
    
    public bool defaultAnimation = true;

    public override void Interact()
    {
        if (defaultAnimation)
            GetComponentInParent<Animator>().Play("BasicButtonClickAnim1");

        GetComponent<AudioSource>().Play();

        camWaypoint.StartWaypoint();
    }
}
