using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumpy : MonoBehaviour
{

    [SerializeField][Range(1,10)]
    private float jumpForce = 5;

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

}
