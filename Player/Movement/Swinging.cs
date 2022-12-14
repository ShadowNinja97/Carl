using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swinging : MonoBehaviour //Fuck this script
{
    [Header("References")]
    public LineRenderer lr;
    public Transform gunTip, cam, player;
    public LayerMask whatIsGrappleable;
    public PlayerMovement pm;
    public GameObject gunHolder;
    public GameObject grappleGun;

    [Header("Swinging")]
    private float maxSwingDistance = 25f;
    private Vector3 swingPoint;
    private SpringJoint joint;

    [Header("OdmGear")]
    public Transform orientation;
    public Rigidbody rb;
    public float horizontalThrustForce;
    public float forwardThrustForce;
    public float extendCableSpeed;

    [Header("Prediction")]
    public RaycastHit predictionHit;
    public float predictionSphereCastRadius;
    public Transform predictionPoint;

    private Vector3 currentGrapplePosition;

    [Header("Input")]
    public KeyCode swingKey = KeyCode.Mouse0;
    


    void Update()
    {
        
        if (Input.GetKeyDown(swingKey)) StartSwing();
        if (Input.GetKeyUp(swingKey)) StopSwing();
        CheckForSwingPoints();
        if (joint != null) OdmGearMovement();
    }
    private void LateUpdate()
    {
        DrawRope();
    }

    private void StartSwing()
    {
        

        // return if predictionHit not found
        if (predictionHit.point == Vector3.zero) return;

        

        // deactivate active grapple
        if (GetComponent<Grappling>() != null)
            GetComponent<Grappling>().StopGrapple();
        pm.ResetRestrictions();
        lr.enabled = true;
        pm.swinging = true;

        grappleGun.SetActive(true);
        gunHolder.SetActive(false);

        swingPoint = predictionHit.point;
        joint = player.gameObject.AddComponent<SpringJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = swingPoint;

        float distanceFromPoint = Vector3.Distance(player.position, swingPoint);

        // the distance grapple will try to keep from grapple point. 
        joint.maxDistance = distanceFromPoint * 0.8f;
        joint.minDistance = distanceFromPoint * 0.25f;

        // customize values as you like
        joint.spring = 4.5f;
        joint.damper = 7f;
        joint.massScale = 4.5f;

        lr.positionCount = 2;
        currentGrapplePosition = gunTip.position;


        /*RaycastHit hit;
        if(Physics.Raycast(cam.position, cam.forward, out hit, maxSwingDistance, whatIsGrappleable))
        {
            swingPoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = swingPoint;

            float distanceFromPoint = Vector3.Distance(player.position, swingPoint);

            //Distance
            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            //Customizable Values
            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            lr.positionCount = 2;
            currentGrapplePosition = gunTip.position;*/
    
    }

    public void StopSwing()
    {
        grappleGun.SetActive(false);
        gunHolder.SetActive(true);
        lr.enabled = false; 
        pm.swinging = false;
        lr.positionCount = 0;
        Destroy(joint);
    }

    void DrawRope()
    {
        if (!joint) return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, swingPoint, Time.deltaTime * 8f);

        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, swingPoint);
    }

    private void OdmGearMovement()
    {
        // right
        if (Input.GetKey(KeyBinds.right)) rb.AddForce(orientation.right * horizontalThrustForce * Time.deltaTime);
        // left
        if (Input.GetKey(KeyBinds.left)) rb.AddForce(-orientation.right * horizontalThrustForce * Time.deltaTime);

        // forward
        if (Input.GetKey(KeyBinds.forward)) rb.AddForce(orientation.forward * horizontalThrustForce * Time.deltaTime);
        // backwards
        if (Input.GetKey(KeyBinds.backwards)) rb.AddForce(-orientation.forward * horizontalThrustForce * Time.deltaTime);

        // shorten cable
        if (Input.GetKey(KeyBinds.decreaseRope))
        {
            Vector3 directionToPoint = swingPoint - transform.position;
            rb.AddForce(directionToPoint.normalized * forwardThrustForce * Time.deltaTime);

            float distanceFromPoint = Vector3.Distance(transform.position, swingPoint);

            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;
        }
        // extend cable
        if (Input.GetKey(KeyBinds.increaseRope))
        {
            float extendedDistanceFromPoint = Vector3.Distance(transform.position, swingPoint) + extendCableSpeed;

            joint.maxDistance = extendedDistanceFromPoint * 0.8f;
            joint.minDistance = extendedDistanceFromPoint * 0.25f;
        }
    }

    private void CheckForSwingPoints()
    {
        if (joint != null) return;

        RaycastHit sphereCastHit;
        Physics.SphereCast(cam.position, predictionSphereCastRadius, cam.forward,
                            out sphereCastHit, maxSwingDistance, whatIsGrappleable);
        RaycastHit sphereCheck;
        Physics.Raycast(cam.position, sphereCastHit.point - cam.position, out sphereCheck, maxSwingDistance);

        RaycastHit raycastHit;
        Physics.Raycast(cam.position, cam.forward,
                            out raycastHit, maxSwingDistance);

        Vector3 realHitPoint;

        // Option 1 - Direct Hit
        if (raycastHit.point != Vector3.zero && raycastHit.collider.tag == "GrapplePoint")
            realHitPoint = raycastHit.point;

        // Option 2 - Indirect (predicted) Hit
        else if (sphereCheck.point != Vector3.zero && sphereCheck.collider.tag == "GrapplePoint")
            realHitPoint = sphereCheck.point;

        // Option 3 - Miss
        else
            realHitPoint = Vector3.zero;

        

        if (Physics.Raycast(cam.position, sphereCastHit.point - cam.position, maxSwingDistance) && sphereCheck.collider.tag != "GrapplePoint")
            sphereCheck = new RaycastHit();
        if (Physics.Raycast(cam.position, cam.forward, maxSwingDistance) && raycastHit.collider.tag != "GrapplePoint")
            raycastHit = new RaycastHit();


        // realHitPoint found
        if (realHitPoint != Vector3.zero)
        {
            if(GetComponent<Grappling>().gunOn)
                predictionPoint.gameObject.SetActive(true);
            else
                predictionPoint.gameObject.SetActive(false);
            predictionPoint.position = realHitPoint;
        }
        // realHitPoint not found
        else
        {
            predictionPoint.gameObject.SetActive(false);
        }

        predictionHit = raycastHit.point == Vector3.zero ? sphereCheck : raycastHit;
        //predictionHit = raycastHit.point == Vector3.zero ? sphereCheck : raycastHit;
    }

}
