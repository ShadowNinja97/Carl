using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
    This is a base class.
    
    DO NOT ATTACH THIS TO ANY OBJECT

    For other interactible objects that require you to look and click, make sure the script extends Interactible instead of MonoBehaviour.
 */


public abstract class Interactable : MonoBehaviour
{
    public float radius = 3f;

    public abstract void Interact();


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

}
