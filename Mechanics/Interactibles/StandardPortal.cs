using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StandardPortal : MonoBehaviour
{
    
    public Transform teleportPosition;

    public bool visualConnection = false;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        /*audioSource.spatialBlend = 0f;
        audioSource.PlayOneShot(PortalData.portalStartup);
        audioSource.spatialBlend = 1f;*/
        StartCoroutine(PortalStartup());
    }

    private void OnTriggerEnter(Collider other)
    {
        
        other.transform.parent.position = teleportPosition.position;
        
        PlayerCam cam = GameObject.FindObjectOfType<PlayerCam>();

        Quaternion targetRotation = new Quaternion(teleportPosition.eulerAngles.x, teleportPosition.eulerAngles.y, teleportPosition.eulerAngles.z, 1);
        //cam.interuption = true;
        //cam.OverrideMousePos(targetRotation.eulerAngles.x, targetRotation.eulerAngles.y);
        cam.OverrideMousePos(teleportPosition.eulerAngles.x, teleportPosition.eulerAngles.y);

        other.GetComponentInParent<Rigidbody>().velocity = Vector3.zero;
        //cam.interuption = false;

        StartCoroutine(PortalFlashFade());
        

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if(visualConnection)
            Gizmos.DrawLine(transform.position, teleportPosition.position);
    }

    IEnumerator PortalStartup()
    {
        audioSource.maxDistance = 20;
        //audioSource.spatialBlend = 0f;
        audioSource.PlayOneShot(PortalData.portalTravel); //Sounds better imo. Can change to .portalStartup if prefered.
        
        yield return new WaitForSeconds(4f);
        //audioSource.spatialBlend = 1f;
        audioSource.maxDistance = 4;
        audioSource.clip = PortalData.portalAmbience;
        audioSource.loop = true;
        audioSource.Play();

    }

    IEnumerator PortalFlashFade()
    {
        
        PortalData.portalFlash.DOFade(1f, 0.1f);
        //audioSource.maxDistance = 20;
        audioSource.spatialBlend = 0f;
        audioSource.PlayOneShot(PortalData.portalTravel);
        
        yield return new WaitForSeconds(0.1f);
        PortalData.portalFlash.DOFade(0f, 0.5f);

        yield return new WaitForSeconds(1.4f);
        audioSource.spatialBlend = 1f;
        //audioSource.maxDistance = 4;
    }
}
