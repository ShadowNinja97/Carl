using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SphereCollider))]
public class SuckyBoy : MonoBehaviour
{
    public float gravityPull = 0.78f;
    public static float gravityRadius = 1f;

    private void Awake()
    {
        gravityRadius = GetComponent<SphereCollider>().radius;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.attachedRigidbody)
        {
            float gravityIntensity = (Vector3.Distance(transform.position, other.transform.position) / gravityRadius);

            other.attachedRigidbody.AddForce((transform.position - other.transform.position) * gravityIntensity * other.attachedRigidbody.mass * gravityPull * Time.smoothDeltaTime);
            Debug.DrawRay(other.transform.position, transform.position - other.transform.position);
        }
    }
}
