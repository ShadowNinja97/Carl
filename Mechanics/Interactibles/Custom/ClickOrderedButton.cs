using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickOrderedButton : Interactable
{

    public int buttonNumber;

    [SerializeField]
    private OrderedButtonController controller;

    public override void Interact()
    {
        GetComponent<AudioSource>().Play();
        GetComponentInParent<Animator>().Play("BasicButtonClickAnim1");
        //Debug.Log("Interacted");
        controller.NextButton(buttonNumber);
    }
}
