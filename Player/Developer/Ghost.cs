using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public KeyCode ghostToggle = KeyCode.H;
    public Transform orientation;

    public float flySpeed;

    private bool flying;

    private float maxSpeed = 30;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirecton;

    Rigidbody rb;
    void Start()
    {
        flying = false;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
            KeyBinds.sprint = KeyCode.R;

        if (Input.GetKeyDown(ghostToggle))
        {
            TimeTrials tt = FindObjectOfType<TimeTrials>();
            if(tt!=null)
            {
                tt.isValid = false;
            }
            flying = !flying;
            rb.velocity = Vector3.zero;

        }
        GetComponentInChildren<CapsuleCollider>().enabled = !flying;
        GetComponentInChildren<MeshRenderer>().enabled = !flying;
        
        GetComponent<PlayerMovement>().enabled = !flying;
        rb.useGravity = !flying;
        if (flying)
        {
            GetInput();
            MovePlayer();
        }

        if(rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        }
    }


    private void GetInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (!Input.anyKey)
        {
            rb.velocity = Vector3.zero;
        }
        if (Input.GetKey(KeyBinds.jump))
            rb.AddForce(Vector3.up * flySpeed * 10f, ForceMode.Force);
        if (Input.GetKey(KeyBinds.crouch))
            rb.AddForce(Vector3.down * flySpeed * 10f, ForceMode.Force);
    }


    private void MovePlayer()
    {
        moveDirecton = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rb.AddForce(moveDirecton.normalized * flySpeed * 10f, ForceMode.Force);
    }


}
