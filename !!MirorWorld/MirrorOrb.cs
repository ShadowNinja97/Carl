using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorOrb : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            FindObjectOfType<MirrorCore>().ToggleMirrorWorld();

    }
    
}
