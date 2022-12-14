using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugFly : MonoBehaviour
{

    public KeyCode toggleFly = KeyCode.M;
    public KeyCode ascend = KeyCode.Space;
    public KeyCode descend = KeyCode.C;

    public Transform orientation;

    public float flySpeed;

    private bool flying;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirecton;

    Rigidbody rb;

    void Start()
    {
        flying = false;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
            KeyBinds.sprint = KeyCode.R;

        if (Input.GetKeyDown(toggleFly))
        {
            flying = !flying;
            rb.velocity = Vector3.zero;
        }
        GetComponent<PlayerMovement>().enabled = !flying;
        rb.useGravity = !flying;
        if (flying)
        {
            GetInput();
            MovePlayer();
        }
    }


    private void GetInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(!Input.anyKey)
        {
            rb.velocity = Vector3.zero;
        }
        if (Input.GetKey(ascend))
            rb.AddForce(Vector3.up * flySpeed * 10f, ForceMode.Force);
        if (Input.GetKey(descend))
            rb.AddForce(Vector3.down * flySpeed * 10f, ForceMode.Force);
    }


    private void MovePlayer()
    {
        moveDirecton = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rb.AddForce(moveDirecton.normalized * flySpeed * 10f, ForceMode.Force);
    }
}
