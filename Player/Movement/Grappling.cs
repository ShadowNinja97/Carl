using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappling : MonoBehaviour
{

    [Header("References")]
    private PlayerMovement pm;
    public Transform cam;
    public Transform gunTip;
    public LayerMask whatIsGrappleable;
    public LineRenderer lr;
    public GameObject gunHolder;
    public GameObject grappleGun;

    [Header("Grappling")]
    public float maxGrappleDistance;
    public float grappleDelayTime;
    public float overshootYAxis;
    
    private Vector3 grapplePoint;
    public bool canGrapple;

    [Header("Cooldown")]
    public float grapplingCooldown;
    private float grapplingCooldownTimer;

    [Header("Input")]
    public KeyCode grappleKey = KeyCode.Mouse1;
    public KeyCode removeGun = KeyCode.Backspace;

    private bool grappling;

    [Header("TestRemove")]
    public GameObject gun;
    public GameObject reticle;
    public bool gunOn = true;

    


    private void Start()
    {
        pm = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(removeGun))
        {
            gunOn = !gunOn;
            gun.SetActive(gunOn);
            reticle.SetActive(gunOn);
        }

        if (Input.GetKeyDown(grappleKey) && canGrapple)
            StartGrapple();
        if (grapplingCooldownTimer > 0)
            grapplingCooldownTimer -= Time.deltaTime;
        
    }
    private void LateUpdate()
    {
        if (grappling)
            lr.SetPosition(0, gunTip.position);
    }

    private void StartGrapple()
    {
        if (grapplingCooldownTimer > 0)
            return;

        grappleGun.SetActive(true);
        gunHolder.SetActive(false);

        lr.positionCount = 2;
        grappling = true;

        pm.freeze = true;

        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, maxGrappleDistance, whatIsGrappleable))
        {
            grapplePoint = hit.point;

            Invoke(nameof(ExecuteGrapple), grappleDelayTime);
        }
        else
        {
            grapplePoint = cam.position + cam.forward * maxGrappleDistance;
            Invoke(nameof(StopGrapple), grappleDelayTime);
        }

        lr.enabled = true;
        lr.SetPosition(1, grapplePoint);
    }

    private void ExecuteGrapple()
    {
        pm.freeze = false;

        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);

        float grapplePointRelativeYPos = grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + overshootYAxis;

        if (grapplePointRelativeYPos < 0) highestPointOnArc = overshootYAxis;

        pm.JumpToPosition(grapplePoint, highestPointOnArc);

        Invoke(nameof(StopGrapple), 1f);
    }

    public void StopGrapple()
    {
        grappleGun.SetActive(false);
        gunHolder.SetActive(true);

        pm.freeze = false;

        grappling = false;
        
        grapplingCooldownTimer = grapplingCooldown;
        
        lr.enabled = false;
    }

} 
