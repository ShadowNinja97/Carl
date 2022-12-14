using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpookyBoy : MonoBehaviour
{

    public float lightRange = 15;

    public GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < lightRange)
        {
            RenderSettings.ambientLight = Color.black;
            RenderSettings.ambientIntensity = 0;
            RenderSettings.reflectionIntensity = 0;
            
        }
        else
        {
            RenderSettings.ambientIntensity = 1;
            RenderSettings.reflectionIntensity = 1;

        }
    }
}
