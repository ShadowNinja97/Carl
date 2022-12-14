using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MirrorShiftOrb : MonoBehaviour
{
    public GameObject sliderObj;
    public Slider shiftPowerSldier;

    public float powerDuration;

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(UsingPower());
    }


    IEnumerator UsingPower()
    {
        AttemptDisable();
        MirrorCore.shiftPower = true;
        sliderObj.SetActive(true);
        shiftPowerSldier.value = 1;
        float z = powerDuration;
        while (z > 0)
        {
            float progress = Mathf.Clamp01(z / powerDuration);
            shiftPowerSldier.value = progress;
            z -= Time.deltaTime;
            yield return null;
        }

        MirrorCore.shiftPower = false;
        sliderObj.SetActive(false);
        AttemptEnable();
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
    }
}
