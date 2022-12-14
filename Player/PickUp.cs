using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public Transform fpsCam;
    public GameObject fpsCamObject, gunContainer, recoilCamContainer;

    public float pickUpRange;

    public static bool slotFull; 

    public RaycastHit lookingPoint;

    public KeyCode dropKey = KeyCode.Q;
    //public KeyCode grab = KeyCode.F;

    // Replacements for GetComponent<>()
    private WeaponSwitcher weaponSwitcher;
    private CameraPrompt cameraPrompt;
    private GameObject lookObject;
    private Transform player;

    private void Start() {
        /* if(gunContainer.GetComponent<WeaponSwitcher>().getSelectedWeapon() == gameObject) {
            equipped = true;
        } */

        player = transform;
        weaponSwitcher = gunContainer.GetComponent<WeaponSwitcher>();
        cameraPrompt = fpsCamObject.GetComponent<CameraPrompt>();
    }

    private void FixedUpdate() {
        // Check if player in range and "F" pressed. P.S. Screw you Tyler - Fuck you
                //If "Interact" key is pressed.
        if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out lookingPoint)) {
            lookObject = lookingPoint.collider.gameObject;
            if(lookingPoint.distance <= pickUpRange && Input.GetKeyDown(KeyBinds.interact)) {
                Debug.Log("Trying to pick up " + lookingPoint.collider.name);
                if(lookingPoint.collider.CompareTag("Weapon")) {
                    if(cameraPrompt.heldObject != cameraPrompt.defaultHeldObject && cameraPrompt.heldObject.GetComponent<PickUpController>().itemName.Equals(lookObject.GetComponent<PickUpController>().itemName) && !cameraPrompt.heldObject.name.Equals(lookObject.name)) {
                        cameraPrompt.heldObject.GetComponent<GunAbstract>().AddAmmo(lookObject.GetComponent<GunAbstract>().GetCurrentAmmo());
                        Destroy(lookObject);
                    }
                    else if(cameraPrompt.heldObject != cameraPrompt.defaultHeldObject && weaponSwitcher.getHolsteredWeapon() != null && weaponSwitcher.getHolsteredWeapon().GetComponent<PickUpController>().itemName.Equals(lookObject.GetComponent<PickUpController>().itemName) && !cameraPrompt.heldObject.name.Equals(lookObject.name)) {
                        weaponSwitcher.getHolsteredWeapon().GetComponent<GunAbstract>().AddAmmo(lookObject.GetComponent<GunAbstract>().GetCurrentAmmo());
                        Destroy(lookObject);
                    }
                    else {
                        // int index = fpsCamObject.GetComponent<CameraPrompt>().defaultHeldObject.transform.GetSiblingIndex();
                        if (cameraPrompt.heldObject != cameraPrompt.defaultHeldObject && gunContainer.transform.childCount > 1) {
                            // index = fpsCamObject.GetComponent<CameraPrompt>().heldObject.transform.GetSiblingIndex();
                            cameraPrompt.heldObject.GetComponent<PickUpController>().Drop();
                        }
                        else if (gunContainer.transform.childCount == 1) {
                            cameraPrompt.heldObject.GetComponent<PickUpController>().Swap();
                        }
                        lookObject.GetComponent<PickUpController>().SetGunContainer(gunContainer);
                        weaponSwitcher.SetSelectedWeapon(lookingPoint.collider.gameObject);
                        // lookingPoint.collider.gameObject.transform.SetSiblingIndex(index);
                        lookObject.GetComponent<PickUpController>().PickUpItem();
                        lookObject.GetComponent<PickUpController>().SetFPSCam(fpsCam, fpsCamObject);
                        lookObject.GetComponent<GunAbstract>().SetGunCam(recoilCamContainer, fpsCamObject.GetComponent<Camera>(), gameObject);
                        cameraPrompt.setHeldObject(lookingPoint.collider.gameObject);
                    }
                }
            }

        }
        // Drop if "backspace" is pressed
        /* if(Input.GetKeyDown(dropKey)) {
            fpsCamObject.GetComponent<CameraPrompt>().heldObject.GetComponent<PickUpController>().Drop();
            fpsCamObject.GetComponent<CameraPrompt>().setDefaultObject();
        } */
    }
}
