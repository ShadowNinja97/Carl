using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTime : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Time.timeScale = 0.5f;
            PlayerCam pc = FindObjectOfType<PlayerCam>();
            pc.sensX *= 2;
            pc.sensY *= 2;
        }
        if (Input.GetKeyUp(KeyCode.B))
        {
            Time.timeScale = 1f;
            PlayerCam pc = FindObjectOfType<PlayerCam>();
            pc.sensX /= 2;
            pc.sensY /= 2;
        }
    }
}
