using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEMP_SwitchCrosshair : MonoBehaviour
{
    public GameObject c1;
    public GameObject c2;
    private bool c2on = false;
    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha7))
            c2on = !c2on;
        c1.SetActive(!c2on);
        c2.SetActive(c2on);
    }

}
