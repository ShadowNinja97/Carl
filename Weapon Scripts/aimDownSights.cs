using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AimDownSights : MonoBehaviour
{
    public float zoomMagnifier = 1.4f;
    public float ADSTime;

    Camera recoilCam;
    private GunAbstract gun;
    private bool inADS;
    private Recoil recoil;
    private float cameraBaseFOV;
    private GameObject player;
    private PlayerCam playerCam;
    private float zoomFOV;
    // Start is called before the first frame update
    void Awake()
    {
        gun = gameObject.GetComponent<GunAbstract>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gun.GetRecoilCamContainer()) {
            recoil = gun.GetRecoilCamContainer().GetComponent<Recoil>();
            recoilCam = gun.GetfpsCam();
            cameraBaseFOV = recoilCam.GetComponent<CameraPrompt>().getFOV();
            player = gun.GetPlayer();
            playerCam = recoilCam.GetComponent<PlayerCam>();
            zoomFOV = cameraBaseFOV/zoomMagnifier;
        
            if (Input.GetKey(KeyBinds.ADS) && recoilCam.fieldOfView != zoomFOV && !player.GetComponent<PlayerMovement>().sprinting) {
                inADS = true;
                playerCam.DoFov(zoomFOV, ADSTime);
            }
            else if (recoilCam.fieldOfView != cameraBaseFOV && !player.GetComponent<PlayerMovement>().sprinting ) {
                inADS = false;
                playerCam.DoFov(cameraBaseFOV, ADSTime);
                /*recoilCam.DOFieldOfView(cameraBaseFOV, ADSTime);
                Debug.Log("FOV: " + recoilCam.fieldOfView); */
            }
            recoil.setADS(inADS);
        }
    }
}
