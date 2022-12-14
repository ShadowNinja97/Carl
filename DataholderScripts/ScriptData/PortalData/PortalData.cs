using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortalData : MonoBehaviour
{

    public static Image portalFlash;

    public static AudioClip portalStartup;
    public static AudioClip portalTravel;
    public static AudioClip portalAmbience;

    public Image portalFlashObj;

    public AudioClip portalStartupSound;
    public AudioClip portalTravelSound;
    public AudioClip portalAmbienceSound;

    private void Start()
    {
        AssignValues();
    }

    private void AssignValues()
    {
        portalFlash = portalFlashObj;
        
        portalStartup = portalStartupSound;
        portalTravel = portalTravelSound;
        portalAmbience = portalAmbienceSound;
    }

}
