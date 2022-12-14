using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointInteractionMover : MonoBehaviour
{
    [SerializeField] private WaypointSystem waypoints;

    [SerializeField] private float moveSpeed = 5f;

    [SerializeField] private int startPoint = 0;

    [SerializeField] private float distanceThreshold = 0.1f;

    [SerializeField] private float pauseDuration = 0f;

    [SerializeField]
    private Transform startWaypoint;
    [SerializeField]
    private Transform lastWaypoint;
    [SerializeField]
    private Transform currentWaypoint;
    [SerializeField]
    private Transform hiddenWaypoint;

    private bool canMove = false;
    private bool started = false;

    void Start()
    {
        canMove = false;
        currentWaypoint = waypoints.SetInitialWaypoint(startPoint);
        startWaypoint = currentWaypoint;
        lastWaypoint = waypoints.SetInitialWaypoint(waypoints.GetMaxWaypoint());
        hiddenWaypoint = currentWaypoint;

        transform.position = currentWaypoint.position;


        //Set Next Waypoint Target
        //currentWaypoint = null;

    }

    void Update()
    {

        if (canMove && currentWaypoint != null)
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, moveSpeed * Time.deltaTime);

        try
        {
            if (Vector3.Distance(transform.position, currentWaypoint.position) < distanceThreshold)
            {

                if (waypoints.mode == WaypointSystem.Mode.Once)
                {
                    if (startWaypoint == currentWaypoint)
                    {
                        currentWaypoint = null;
                        waypoints.isReversed = false;
                        canMove = false;
                    }
                    if (lastWaypoint == currentWaypoint)
                    {
                        currentWaypoint = null;
                        waypoints.isReversed = true;
                        canMove = false;
                    }
                }

                if (waypoints.settings.HasFlag(WaypointSystem.Settings.PauseAtDesination) && started)
                {
                    currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
                    if (currentWaypoint != null)
                        hiddenWaypoint = currentWaypoint;
                    StartCoroutine(DelayedMove(pauseDuration));
                }
                else
                {
                    currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
                    if(currentWaypoint!=null)
                        hiddenWaypoint = currentWaypoint;
                    

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

    public void InteractionStart()
    {
        canMove = true;
        started = true;
        if(waypoints.mode == WaypointSystem.Mode.Once)
        currentWaypoint = waypoints.GetNextWaypoint(hiddenWaypoint);
    }
}
