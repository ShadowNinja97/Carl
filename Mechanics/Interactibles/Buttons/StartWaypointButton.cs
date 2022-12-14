using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartWaypointButton : Interactable
{
    
    public WaypointInteractionMover[] waypointsToActivate;

    public bool defaultAnimation = true;

    public override void Interact()
    {
        if (defaultAnimation)
            GetComponentInParent<Animator>().Play("BasicButtonClickAnim1");

        GetComponent<AudioSource>().Play();

        foreach (WaypointInteractionMover waypoint in waypointsToActivate)
        {
            waypoint.InteractionStart();
        }
    }
}
