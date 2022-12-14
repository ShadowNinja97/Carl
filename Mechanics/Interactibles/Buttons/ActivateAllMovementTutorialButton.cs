using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateAllMovementTutorialButton : Interactable
{
    public MonoBehaviour[] scripts;


    public override void Interact()
    {
        GetComponent<AudioSource>().Play();
        GetComponentInParent<Animator>().Play("BasicButtonClickAnim1");
        for (int i = 0; i < scripts.Length; i++)
        {
            scripts[i].enabled = true;
        }
    }
}
