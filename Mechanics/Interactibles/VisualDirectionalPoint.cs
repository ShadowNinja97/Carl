using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualDirectionalPoint : MonoBehaviour
{

    public float radius = 1;
    public float length = 3f;


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
        Vector3 forward = transform.position + transform.forward*length;
        //forward = new Vector3(forward.x, forward.y, forward.z + 3);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, forward);
    }

}
