using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerMovement : MonoBehaviour
{
    #region Variables
    [Header("Movement")]
    public float moveSpeed;
    [HideInInspector]
    public float currentSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float slideSpeed;
    public float climbSpeed;
    public float wallRunSpeed;
    public float swingSpeed;

    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;

    public float speedIncreaseMultiplier;
    public float slopeIncreaseMultiplier;

    public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool canJump;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    
    private bool toggleSprinting = false;

    [Header("Debug KeyBinds")]
    public bool inSandbox;
    [Tooltip("Go to 0, 10, 0")]
    public KeyCode resetPosition = KeyCode.Tab;
    public KeyCode slidePosition = KeyCode.Alpha9;
    public KeyCode slidePosition2 = KeyCode.Alpha8;
    public KeyCode downToEarth = KeyCode.X;
    

    [Header("GroundCheck")]
    public float playerHeight;
    public LayerMask ground;
    public bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    [Header("Debug Displays")]
    public GameObject velocityHud;
    bool veloHudOn = false;

    //    [Space(20)]
    [Header("References")]
    public Climbing climbingScript;
    public Transform orientation;
    public Transform playerObj;
    public PlayerCam pc;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirecton;

    Rigidbody rb;

    public MovementState state;

    [HideInInspector]
    public bool smoothSpeedOverride = false;

    public enum MovementState
    {
        freeze,
        walking,
        swinging,
        sprinting,
        wallrunning,
        climbing,
        crouching,
        sliding,
        air
    }

    public bool sliding;
    public bool climbing;
    public bool wallrunning;
    public bool freeze;
    public bool activeGrapple;
    public bool swinging;
    public bool sprinting;

    [HideInInspector]
    public bool OnSpeedPad;
    [HideInInspector]
    public float currentSpeedPadPower;

    #endregion
    private void Start()
    {

        currentSpeed = moveSpeed;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        canJump = true;
        startYScale = transform.localScale.y;
    }

    private void Update()
    {
        //Checks if player is grounded
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, ground);

        GetInput();
        SpeedControl();
        StateHandler();
        VelocityHudOn();

        //Handle Drag
        if (grounded && !activeGrapple)
            rb.drag = groundDrag;
        else
            rb.drag = 0f;
        
    }


    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if ((Input.GetKey(KeyBinds.jump) || Input.mouseScrollDelta.magnitude > 0) && canJump && grounded)
        {
            canJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
        if (Input.GetKeyDown(KeyBinds.crouch))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }
        if (Input.GetKeyUp(KeyBinds.crouch))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            //rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }
        if (Input.GetKeyDown(resetPosition) && inSandbox)
            transform.position = new Vector3(0, 10, 0);
        if (Input.GetKeyDown(slidePosition) && inSandbox)
        {
            rb.velocity = Vector3.zero;
            transform.position = new Vector3(-30, 50, -90);
        }
        if (Input.GetKeyDown(slidePosition2) && inSandbox)
        {
            rb.velocity = Vector3.zero;
            transform.position = new Vector3(-85, 35, 32);

        }
        if (Input.GetKeyDown(downToEarth) && inSandbox)
        {
            rb.velocity = Vector3.zero;
            transform.position = new Vector3(transform.position.x, 5, transform.position.z);
        }


        if (Input.GetKeyDown(KeyCode.G))
            veloHudOn = !veloHudOn;

        if (Input.GetKeyDown(KeyBinds.sprint) && grounded)
        {
            pc.DoFov(100, 0.25f);
            toggleSprinting = false;
            sprinting = true;
        }

        if (Input.GetKeyUp(KeyBinds.sprint) && grounded) {
            if(!Input.GetKey(KeyBinds.ADS)) { //Whatever was here broke sprint fov.
                pc.DoFov(80, 0.25f);
            }
            // pc.DoFov(80, 0.25f);
            sprinting = false;
        }
        
        if (!Input.GetKey(KeyBinds.sprint) && grounded && !toggleSprinting) {
            if(!Input.GetKey(KeyBinds.ADS)) { //same as above
                pc.DoFov(80, 0.25f);
            }
            // pc.DoFov(80, 0.25f);
            sprinting = false;
        }

        if (Input.GetKeyDown(KeyBinds.toggleSprint))
        {
            toggleSprinting = !toggleSprinting;
            sprinting = toggleSprinting;
            if (toggleSprinting) {
                pc.DoFov(100, 0.25f);
            }
            else {
                if(!Input.GetKey(KeyBinds.ADS)) {
                    pc.DoFov(80, 0.25f);
                }
                //pc.DoFov(80, 0.25f); //moved down so the sprinting works.

            }
        }


    }

    private void StateHandler()
    {
        if (freeze) //Change at some point, this is for grappling
        {
            state = MovementState.freeze;
            moveSpeed = 0;
            rb.velocity = Vector3.zero;
        }

        else if (swinging)
        {
            state = MovementState.swinging;
            desiredMoveSpeed = swingSpeed;
        }

        else if (OnSpeedPad)
        {
            desiredMoveSpeed = walkSpeed * currentSpeedPadPower;
            moveSpeed = walkSpeed * currentSpeedPadPower;
        }

        else if (climbing)
        {
            state = MovementState.climbing;
            desiredMoveSpeed = climbSpeed;
        }

        //Mode - Wallrunning
        else if (wallrunning)
        {
            state = MovementState.wallrunning;
            desiredMoveSpeed = wallRunSpeed;
        }

        //Mode - Sliding
        else if (sliding)
        {
            state = MovementState.sliding;
            if (OnSlope() && rb.velocity.y < 0.1f)
                desiredMoveSpeed = slideSpeed;
            else
                desiredMoveSpeed = sprintSpeed;
        }
        //Mode - Crouching
        else if (Input.GetKey(KeyBinds.crouch))
        {
            state = MovementState.crouching;
            desiredMoveSpeed = crouchSpeed;
        }

        //Mode - Sprinting
        else if ((grounded && Input.GetKey(KeyBinds.sprint)) || (grounded && toggleSprinting))
        {
            if(!grounded)
            {
                state = MovementState.air;
                return;
            }
            state = MovementState.sprinting;
            desiredMoveSpeed = sprintSpeed;
        }

        //Mode - Walking
        else if (grounded)
        {
            state = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
        }

        //Mode - Air
        else
        {
            state = MovementState.air;
        }



        //Check if desired move speed is massively different
        if (desiredMoveSpeed > 50f)
        {
            desiredMoveSpeed = 50;
            moveSpeed = desiredMoveSpeed;
        }
        else if ((Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 4f && moveSpeed != 0)|| smoothSpeedOverride)
        {
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed());
        }
        else
        {
            moveSpeed = desiredMoveSpeed;
        }
        lastDesiredMoveSpeed = desiredMoveSpeed;
    }


    private void MovePlayer()
    {
        if (activeGrapple) return;
        if (swinging) return;

        if (climbingScript.exitingWall) return;
        //Calculate Movement Direction
        moveDirecton = orientation.forward * verticalInput + orientation.right * horizontalInput;

        

        //Slope Movement
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection(moveDirecton) * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }


        //Grounded Movement
        else if (grounded)
            rb.AddForce(moveDirecton.normalized * moveSpeed * 10f, ForceMode.Force);

        //Air Movement
        else if (!grounded)
            rb.AddForce(moveDirecton.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);



        //Disable Gravity on Slopes
        if (!wallrunning)
            rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        if (activeGrapple) return;

        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //Limits speed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
        /*
        if (flatVel.magnitude > moveSpeed)
            currentSpeed = moveSpeed/4;
        else
            currentSpeed = moveSpeed;
        */
    }

    private void Jump()
    {
        exitingSlope = true;

        //Reset Y Velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        canJump = true;

        exitingSlope = false;
    }

    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }

    private void VelocityHudOn()
    {
        TextMeshProUGUI displayText = velocityHud.GetComponent<TextMeshProUGUI>();
        displayText.text = "Velocity: \n" + System.Math.Round(rb.velocity.magnitude,3);

        if (veloHudOn)
            velocityHud.SetActive(true);
        else
            velocityHud.SetActive(false);
    }

    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);

            if (OnSlope())
            {
                float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                float slopeAngleIncrease = 1 + (slopeAngle / 90f);

                time += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease;
            }
            else
                time += Time.deltaTime * speedIncreaseMultiplier;

            yield return null;
        }
        moveSpeed = desiredMoveSpeed;
        smoothSpeedOverride = false;
    }

    public Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        float gravity = Physics.gravity.y;
        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity) + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

        return velocityXZ + velocityY;
    }

    private bool enableMovementOnNextTouch;
    public void JumpToPosition(Vector3 targetPos, float trajectoryHeight)
    {
        activeGrapple = true;
        velocityToSet = CalculateJumpVelocity(transform.position, targetPos, trajectoryHeight);
        Invoke(nameof(SetVelocity), 0.1f);

        Invoke(nameof(ResetRestrictions), 3f);
    }

    private Vector3 velocityToSet;
    private void SetVelocity()
    {
        enableMovementOnNextTouch = true;
        rb.velocity = velocityToSet;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (enableMovementOnNextTouch)
        {
            enableMovementOnNextTouch = false;
            ResetRestrictions();
            GetComponent<Grappling>().StopGrapple();
        }
    }
    public void ResetRestrictions()
    {
        activeGrapple = false;
    }

}
