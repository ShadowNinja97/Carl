using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventInteractibleButton : Interactable
{

    [SerializeField]
    private EventTriggerBase eTB;
    public override void Interact()
    {
        GetComponent<AudioSource>().Play();
        GetComponentInParent<Animator>().Play("BasicButtonClickAnim1");
        eTB.Trigger();
    }
}
