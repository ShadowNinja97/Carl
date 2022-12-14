using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TEMP_FOVEnemy : MonoBehaviour
{
    public float radius;
    [Range(0, 360)]
    public float angle;

    public GameObject playerRef;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeePlayer;

    public float speed;
    public float rotateSpeed = 0.1f;
    public float patrolRotateSpeed = 10f;
    Quaternion rotGoal;
    Vector3 direction;


    private float idleTimer;

    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
    }

    private void Update()
    {
        MoveTowardsPlayer();
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;

            FieldOfViewCheck();

        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    canSeePlayer = true;
                else
                    canSeePlayer = false;
            }
            else
                canSeePlayer = false;

        }
        else if (canSeePlayer)
            canSeePlayer = false;
    }

    public void MoveTowardsPlayer()
    {
        if (canSeePlayer)
        {
            //transform.position = Vector3.MoveTowards(transform.position, playerRef.transform.position, speed * Time.deltaTime);
            GetComponent<Rigidbody>().AddForce(transform.position-playerRef.transform.position, ForceMode.Force);

            direction = (playerRef.transform.position - transform.position).normalized;
            direction.y = 0;
            rotGoal = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotGoal, rotateSpeed);
            idleTimer = 0;
        }
        else
        {
            if (idleTimer < 5)
                idleTimer += Time.fixedDeltaTime;
            else
                Idle();
        }
    }

    public void Idle()
    {
        transform.Rotate(0f, patrolRotateSpeed * Time.deltaTime, 0f, Space.Self);
    }

}

#if UNITY_EDITOR
[CustomEditor(typeof(TEMP_FOVEnemy))]
public class TEMP_FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        TEMP_FOVEnemy fov = (TEMP_FOVEnemy)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.radius);

        Vector3 viewAngle1 = DirectionFromAngle(fov.transform.eulerAngles.y, -fov.angle / 2);
        Vector3 viewAngle2 = DirectionFromAngle(fov.transform.eulerAngles.y, fov.angle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle1 * fov.radius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle2 * fov.radius);

        if (fov.canSeePlayer)
        {
            Handles.color = Color.green;
            Handles.DrawLine(fov.transform.position, fov.playerRef.transform.position);
        }
    }


    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
#endif