using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartupDisable : MonoBehaviour
{
    public GameObject[] objects;

    private void Awake()
    {
        foreach (GameObject obj in objects){
            obj.SetActive(false);
        }
    }
}
