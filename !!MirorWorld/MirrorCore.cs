using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
public class MirrorCore : MonoBehaviour
{
    //public Camera mirrorCamera;

    public Image whiteOut;

    public static bool inMirrorWorld;
    private bool canSwitch;

    private TimeTrials tt;

    private AudioSource audioS;

    public AudioClip swapSound;

    public Volume mirrorVolume;

    [Space(10)]
    [Tooltip("Toggle Dimensions using V")]
    public static bool shiftPower = false;


    void Start()
    {
        tt = FindObjectOfType<TimeTrials>();
        canSwitch = true;
        audioS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ManualToggle();

        if (inMirrorWorld)
            MirrorWorld();
        
    }

    public void ManualToggle()
    {
        if (Input.GetKeyDown(KeyCode.V) && canSwitch && shiftPower)
        {
            inMirrorWorld = !inMirrorWorld;
            //mirrorCamera.enabled = inMirrorWorld;
            mirrorVolume.enabled = inMirrorWorld;
            canSwitch = false;
            StartCoroutine(FlashFade());
            MirrorObject[] mirrored = FindObjectsOfType<MirrorObject>();
            foreach (MirrorObject mir in mirrored)
            {
                mir.BasicSwap();
            }
        }
    }


    public void ToggleMirrorWorld()
    {
        inMirrorWorld = !inMirrorWorld;
        //mirrorCamera.enabled = inMirrorWorld;
        mirrorVolume.enabled = inMirrorWorld;
        canSwitch = false;
        StartCoroutine(FlashFade());
        MirrorObject[] mirrored = FindObjectsOfType<MirrorObject>();
        foreach (MirrorObject mir in mirrored)
        {
            mir.BasicSwap();
        }
       
    }

    public void MirrorWorld()
    {
        
    }


    IEnumerator FlashFade()
    {

        whiteOut.DOFade(1f, 0.1f);
        //audioSource.maxDistance = 20;
        audioS.spatialBlend = 0f;
        audioS.PlayOneShot(swapSound);

        yield return new WaitForSeconds(0.1f);
        whiteOut.DOFade(0f, 0.5f);

        yield return new WaitForSeconds(1.4f);
        audioS.spatialBlend = 1f;
        //audioSource.maxDistance = 4;
        canSwitch = true;
    }

}
