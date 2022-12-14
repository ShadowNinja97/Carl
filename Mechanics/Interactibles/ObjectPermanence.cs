using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ObjectPermanence : MonoBehaviour
{


    public enum ComponentType
    {
        MeshCollider,
        BoxCollider,
        Rigidbody
    }

    public ComponentType componentToggle;

    


    private void OnBecameInvisible()
    {
        switch (componentToggle)
        {
            case ComponentType.MeshCollider:
                GetComponent<MeshCollider>().enabled = false;
                break;
        }
        
    }

    IEnumerator activate()
    {
        yield return new WaitForSeconds(0.25f);
        switch (componentToggle)
        {
            case ComponentType.MeshCollider:
                GetComponent<MeshCollider>().enabled = true;
                break;
        }
    }

    private void OnBecameVisible()
    {
        StartCoroutine(activate());
        
    }
}
