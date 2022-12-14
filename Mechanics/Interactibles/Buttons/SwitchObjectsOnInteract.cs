using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchObjectsOnInteract : Interactable
{
    public bool enable = true;

    public GameObject[] objects;

    public override void Interact()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].SetActive(enable);
        }
    }
}
