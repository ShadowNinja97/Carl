using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragGrenade : MonoBehaviour
{
    [Header("Stats")]
    public float damage;
    public float delay = 3f;
    public float radius = 5f;
    public float explosionForce = 700f;
    public float throwForce;
    public float throwUppwardForce;

    [Header("References")]
    public GameObject explosionEffect;
    public LayerMask enemyLayers;
    public LayerMask dynamicLayers;
    public LayerMask blockingLayers;

    // Start is called before the first frame update
    void Awake()
    {
        Invoke("Explode", delay);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Explode() {
        Debug.Log("Explode");

        // Show effect
        Instantiate(explosionEffect, transform.position, transform.rotation);

        //Get nearby objects
        Collider[] effectedObjects = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider nearbyObject in effectedObjects) {

            RaycastHit hit;

            Physics.Linecast(transform.position, nearbyObject.transform.position, out hit, blockingLayers);

            // Check if there are no objects blocking the explosion
            if(hit.collider == nearbyObject) {
                Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
                Debug.Log("Grenade hit " + nearbyObject.name);

                // Add force
                if (dynamicLayers.ContainsLayer(nearbyObject.gameObject.layer)) {

                    if(rb != null) {
                        rb.AddExplosionForce(explosionForce, transform.position, radius);
                    }
                }

                // Damage
                if (enemyLayers.ContainsLayer(nearbyObject.gameObject.layer)) {

                }
            }
        }
        // Destroy grenade
        Destroy(gameObject);
    }
}
