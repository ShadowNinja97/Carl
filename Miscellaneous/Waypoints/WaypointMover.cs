using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointMover : MonoBehaviour
{
    [SerializeField] private WaypointSystem waypoints;

    [SerializeField] private float moveSpeed = 5f;

    [SerializeField] private int startPoint = 0;

    [SerializeField] private float distanceThreshold = 0.1f;

    [SerializeField] private float pauseDuration = 0f;

    private Transform currentWaypoint;

    private bool canMove = true;


    void Start()
    {
        currentWaypoint = waypoints.SetInitialWaypoint(startPoint);
        transform.position = currentWaypoint.position;

        //Set Next Waypoint Target
        currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);

        
    }

    void Update()
    {
        

        if(canMove && currentWaypoint!=null)
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, moveSpeed * Time.deltaTime);

        try
        {
            if (Vector3.Distance(transform.position, currentWaypoint.position) < distanceThreshold)
            {
                if (waypoints.settings.HasFlag(WaypointSystem.Settings.PauseAtDesination))
                {
                    currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
                    StartCoroutine(DelayedMove(pauseDuration));
                }
                else
                {
                    currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
                }
            }
        }
        catch
        {
            return;
        }
        
    }

    IEnumerator DelayedMove(float pause)
    {
        canMove = false;
        yield return new WaitForSeconds(pause);
        canMove = true;
        
    }

}
