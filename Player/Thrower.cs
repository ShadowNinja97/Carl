using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrower : MonoBehaviour
{
    [Header("References")]
    public Transform playerCam;
    public Transform attackPoint;
    public GameObject throwableObject;

    [Header("Settings")]
    public int totalThrowables;
    public float throwCooldown;

    // Throwing
    private float throwForce;
    private float throwUppwardForce;

    bool readyToThrow;

    void Start()
    {
        readyToThrow = true;
    }

    void Update()
    {
        throwForce = throwableObject.GetComponent<FragGrenade>().throwForce;
        throwUppwardForce = throwableObject.GetComponent<FragGrenade>().throwUppwardForce;

        if(Input.GetKeyDown(KeyBinds.throwGrenade) && readyToThrow && totalThrowables > 0) {
            Throw();
        }
    }

    public void Throw() {
        readyToThrow = false;

        // Spawn object
        GameObject throwable = Instantiate(throwableObject, attackPoint.position, playerCam.rotation);

        // Get Rigidbody
        Rigidbody throwableRB = throwable.GetComponent<Rigidbody>();

        // Calculate Direction
        Vector3 forceDirection = playerCam.transform.forward;

        RaycastHit hit;

        if(Physics.Raycast(playerCam.position, playerCam.forward, out hit, 500f)) {
            forceDirection = (hit.point - attackPoint.position).normalized;
        }

        // Add Force
        Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUppwardForce;

        throwableRB.AddForce(forceToAdd, ForceMode.Impulse);

        totalThrowables--;

        // Cooldown
        Invoke(nameof(ResetThrow), throwCooldown);
    }

    private void ResetThrow() {
        readyToThrow = true;
    }

    public void AddGrenades(int i) {
        totalThrowables += i;
    } 
}
