using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunning : MonoBehaviour
{
    [Header("Wallrunning")]
    public LayerMask whatIsWall;
    public LayerMask whatIsGround;
    public float wallRunForce;
    public float wallJumpUpForce;
    public float wallJumpSideForce;
    public float wallClimbSpeed;
    public float maxWallRunTime;
    private float wallRunTimer;

    [Header("Input")]
    //public KeyCode jumpKey = KeyCode.Space;
    public KeyCode upwardsRunKey = KeyCode.LeftShift;
    public KeyCode downwardsRunKey = KeyCode.LeftControl;
    private bool upRunning;
    private bool downRunning;

    private float horizontalInput;
    private float verticalInput;

    [Header("Detection")]
    public float wallCheckDistance;
    public float minJumpHeight;
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;
    private bool wallLeft;
    private bool wallRight;

    [Header("Exiting")]
    private bool exitingWall;
    public float exitWallTime;
    private float exitWallTimer;

    [Header("Gravity")]
    public bool useGravity;
    public float gravityCounterForce;

    [Header("References")]
    public Transform orientation;
    public PlayerCam cam;
    private PlayerMovement pm;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        CheckForWall();
        StateMachine();
    }

    private void FixedUpdate()
    {
        if (pm.wallrunning)
            WallRunningMovement();
    }

    private void CheckForWall()
    {
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallCheckDistance, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallCheckDistance, whatIsWall);
    }

    public bool AboveGround()
    {
        
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
    }

    private void StateMachine()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        upRunning = Input.GetKey(upwardsRunKey);
        downRunning = Input.GetKey(downwardsRunKey);

        //State 1 - WallRunning
        if((wallLeft || wallRight) && verticalInput > 0 && AboveGround() && !exitingWall)
        {
            //Start Wallrunning
            if (!pm.wallrunning)
                StartWallRun();

            if (AboveGround() && Input.GetKeyDown(KeyBinds.jump))
                WallJump();

            if (wallRunTimer > 0)
                wallRunTimer -= Time.deltaTime;

            if (wallRunTimer <= 0)
            {
                exitingWall = true;
                exitWallTimer = exitWallTime;
            }
        }
        //State 2 - Exiting
        else if (exitingWall)
        {
            if (pm.wallrunning)
                StopWallRun();

            if (exitWallTimer > 0)
                exitWallTimer -= Time.deltaTime;

            if (exitWallTimer <= 0)
                exitingWall = false;
            
        }

        //State 3 - None
        else
        {
            if (pm.wallrunning)
                StopWallRun();
        }
    }

    private void StartWallRun()
    {
        pm.wallrunning = true;
        wallRunTimer = maxWallRunTime;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //Camera Effects
        cam.DoFov(90f, 0.25f);
        if (wallLeft) cam.DoTilt(-5f);
        if (wallRight) cam.DoTilt(5f);
    }

    private void WallRunningMovement()
    {
        rb.useGravity = useGravity;
        

        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;

        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        if ((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
            wallForward = -wallForward;

        //Forward Force
        rb.AddForce(wallForward * wallRunForce, ForceMode.Force);

        //Upwards/downwards force
        if (upRunning)
            rb.velocity = new Vector3(rb.velocity.x, wallClimbSpeed, rb.velocity.z);
        if (downRunning)
            rb.velocity = new Vector3(rb.velocity.x, -wallClimbSpeed, rb.velocity.z);

        //Push to Wall
        if (!(wallLeft && horizontalInput > 0) && !(wallRight && horizontalInput < 0))
            rb.AddForce(-wallNormal * 100, ForceMode.Force);

        //Weaken Gravity
        if (useGravity)
            rb.AddForce(transform.up * gravityCounterForce, ForceMode.Force);
    }

    private void StopWallRun()
    {
        pm.wallrunning = false;

        //Reset Camera Effects
        if (!pm.sprinting)
        {
            cam.DoFov(80, 0.25f);
            cam.DoTilt(0f);
        }
        else
        {
            cam.DoFov(100, 0.25f);
            cam.DoTilt(0f);
        }
    }

    private void WallJump()
    {
        exitingWall = true;
        exitWallTimer = exitWallTime;

        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;

        Vector3 forceToApply = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;

        //Add force
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.y);
        rb.AddForce(forceToApply, ForceMode.Impulse);
    }
}
