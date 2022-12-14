using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWaypointMover : MonoBehaviour
{
    [SerializeField] private CameraWaypoint waypoints;

    private Transform currentWaypoint;

    private bool played = false;


    public Camera cutsceneCam;

    private void Start()
    {
        currentWaypoint = waypoints.SetInitialWaypoint(0);

    }



    public void StartWaypoint()
    {
        if (!played)
            StartCoroutine(CameraMove());
    }


    private IEnumerator CameraMove()
    {
        cutsceneCam.enabled = true;

        DisableScripts();
        int pointCount = waypoints.gameObject.transform.childCount;

        for (int i = 0; i < pointCount; i++)
        {
            transform.position = currentWaypoint.position;
            transform.rotation = currentWaypoint.rotation;
            yield return new WaitForSeconds(waypoints.pauseDuration);
            currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);

        }
        
        played = true;
        cutsceneCam.enabled = false;
        ReenableScripts();
        
    }



    private void DisableScripts()
    {
        FindObjectOfType<PlayerCam>().enabled = false;
        FindObjectOfType<MoveCamera>().enabled = false;
    }

    private void ReenableScripts()
    {
        FindObjectOfType<PlayerCam>().enabled = true;
        FindObjectOfType<MoveCamera>().enabled = true;
    }
}
