using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeveloperCheckpoint : MonoBehaviour
{
    [HideInInspector]
    public GameObject checky;
    [HideInInspector]
    public Transform devCheckpoint;

    private void Start()
    {
        checky = new GameObject("Developer Checkpoint");
        devCheckpoint = checky.transform;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            devCheckpoint.position = transform.position;
            devCheckpoint.rotation = transform.rotation;
            GetComponent<PlayerManager>().SetCheckPoint(devCheckpoint);
        }
    }
}
