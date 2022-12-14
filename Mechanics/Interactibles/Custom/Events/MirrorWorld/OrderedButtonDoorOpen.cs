using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderedButtonDoorOpen : EventTriggerBase
{
    [Header("Camera Cutscene")]
    public bool hasCameraCutscene;
    public CameraWaypointMover camWay;
    [Space(10)]
    [Header("Door Settings")]
    public GameObject door;
    private Animator anim;

    public bool standardDoorAnimation = true;
    [HideInInspector]
    public string doorAnimNameOpen;


    private void Start()
    {
        anim = door.GetComponent<Animator>();
    }

    public override void Trigger()
    {

        if (hasCameraCutscene)
            camWay.StartWaypoint();
        if (standardDoorAnimation)
            anim.Play("StandardDoorOpening");
        else
            anim.Play(doorAnimNameOpen);

    }
}
