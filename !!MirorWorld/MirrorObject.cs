using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorObject : MonoBehaviour
{
    public enum Dimension
    {
        Normal,
        Mirror,
        Both
    }
    public Dimension existDimensions;


    public void BasicSwap()
    {
        if (existDimensions == Dimension.Both)
            return;
        else if (existDimensions == Dimension.Mirror)
        {
            if (MirrorCore.inMirrorWorld)
            {

                AttemptEnable();
            }
            else
            {
                AttemptDisable();
            }
        }
        else if (existDimensions == Dimension.Normal)
        {
            if (MirrorCore.inMirrorWorld)
            {
                AttemptDisable();
            }
            else
            {
                AttemptEnable();
            }
        }

    }

    public void AttemptEnable()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = false;

        Collider c = GetComponent<Collider>();
        if (c != null)
            c.enabled = true;

        Renderer r = GetComponent<Renderer>();
        if (r != null)
            r.enabled = true;

        ParticleSystem ps = GetComponent<ParticleSystem>();
        if (ps != null)
            ps.Play();

        TMPro.TextMeshPro tmp = GetComponent<TMPro.TextMeshPro>();
        if (tmp != null)
        {
            tmp.enabled = true;
        }
    }

    public void AttemptDisable()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = true;

        Collider c = GetComponent<Collider>();
        if (c != null)
            c.enabled = false;

        Renderer r = GetComponent<Renderer>();
        if (r != null)
            r.enabled = false;

        ParticleSystem ps = GetComponent<ParticleSystem>();
        if (ps != null)
            ps.Stop();

        TMPro.TextMeshPro tmp = GetComponent<TMPro.TextMeshPro>();
        if (tmp != null)
        {
            tmp.enabled = false;
        }
    }


}
