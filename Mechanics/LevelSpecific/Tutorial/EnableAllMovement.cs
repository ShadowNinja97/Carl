using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableAllMovement : MonoBehaviour
{
    public MonoBehaviour[] scripts;



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            for (int i = 0; i < scripts.Length; i++)
            {
                scripts[i].enabled = true;
            }
        }
    }
}
