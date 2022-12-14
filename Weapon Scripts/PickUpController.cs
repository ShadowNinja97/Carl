using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    private GunAbstract gunScript;
    private AimDownSights ADSScript;

    public Transform player, fpsCam;
    public GameObject fpsCamObject, gunContainer;

    public float pickUpRange, dropForwardForce, dropUpwardsForce;

    public bool equipped;
    public static bool slotFull; 

    public string itemName;

    public RaycastHit lookingPoint;

    Rigidbody rb;
    BoxCollider coll;

    private void Start() {
        // Setup
        rb = gameObject.GetComponent<Rigidbody>();
        coll = gameObject.GetComponent<BoxCollider>();
        gunScript = gameObject.GetComponent<GunAbstract>();
        ADSScript = gameObject.GetComponent<AimDownSights>();

        Debug.Log(gunScript.name);

        /* if(gunContainer.GetComponent<WeaponSwitcher>().getSelectedWeapon() == gameObject) {
            equipped = true;
        } */

        if(!equipped) {
            gunScript.enabled = false;
            ADSScript.enabled = false;
            rb.isKinematic = false;
            coll.isTrigger = false;
        }
        else if(equipped) {
            gunScript.enabled = true;
            ADSScript.enabled = true;
            rb.isKinematic = true;
            coll.isTrigger = true;
            slotFull = true;
            fpsCamObject.GetComponent<CameraPrompt>().setHeldObject(gameObject);
        }
    }

    private void FixedUpdate() {
        // Check if player in range and "F" pressed. P.S. Screw you Tyler
        /* itemDistanceToPlayer = player.position - transform.position;

        if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out lookingPoint)) {
            if(itemDistanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.F) && lookingPoint.collider.CompareTag("Weapon")) {
                if (lookingPoint.transform == gameObject.transform) {
                    if(fpsCamObject.GetComponent<CameraPrompt>().heldObject.GetComponent<PickUpController>().itemName.Equals(itemName) && !fpsCamObject.GetComponent<CameraPrompt>().heldObject.name.Equals(gameObject.name)) {
                        fpsCamObject.GetComponent<CameraPrompt>().heldObject.GetComponent<GunBase>().addAmmo(gunScript.getCurrentAmmo());
                        Destroy(gameObject);
                    }
                    else {
                        PickUp();
                    }
                }
                else if (equipped && !itemName.Equals(lookingPoint.collider.GetComponent<PickUpController>().itemName)) {
                    Drop();
                }
            }

        } */

        // Change layer
        if (equipped && gameObject.layer != LayerMask.NameToLayer("Weapon")) {
            SetLayerofChildren(transform, "Weapon");
            /*gameObject.layer = LayerMask.NameToLayer("Weapon");
            foreach (Transform child in transform) {
                child.gameObject.layer = LayerMask.NameToLayer("Weapon");
            } */
        }
        else if (!equipped && gameObject.layer == LayerMask.NameToLayer("Weapon")) {
            SetLayerofChildren(transform, "Dropped Weapon");
            // gameObject.layer = LayerMask.NameToLayer("Dropped Weapon");
            // foreach (Transform child in transform) {
            //     child.gameObject.layer = LayerMask.NameToLayer("Dropped Weapon");
            // }
        }
    }

    public void PickUpItem() {
        equipped = true;
        slotFull = true;

        gameObject.GetComponent<SphereCollider>().enabled = false;

        // Make weapon a child of the camera and move it to the default position
        transform.SetParent(gunContainer.transform);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;
        
        // Make Rigidbody kinematic and BoxCollider a trigger
        rb.isKinematic = true;
        coll.isTrigger = true;

        //Enable script
        gunScript.enabled = true;
        ADSScript.enabled = true;
    }

    public void Swap() {
        gunContainer.GetComponent<WeaponSwitcher>().SetHolsteredWeapon(gameObject);
        equipped = false;
        gunScript.enabled = false;
        ADSScript.enabled = false;
    }

    public void Drop() {
        equipped = false;
        slotFull = false;

        gameObject.GetComponent<SphereCollider>().enabled = false;

        /* fpsCam = null;
        fpsCamObject = null;
        gunScript.setGunCam(null, null, null); */
        
        // Setparent to null
        transform.SetParent(null);

        // Allow gun to move
        rb.isKinematic = false;
        coll.isTrigger = false;

        // Gun carries momentum of player
        // rb.velocity = player.GetComponent<Rigidbody>().velocity;

        // AddForce
        rb.velocity = player.GetComponent<Rigidbody>().velocity;
        rb.AddForce(fpsCam.forward * dropForwardForce, ForceMode.Impulse);
        rb.AddForce(fpsCam.up * dropUpwardsForce, ForceMode.Impulse);

        // Add random rotation
        rb.AddTorque(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f))*10);

        // Disable script
        gunScript.enabled = false;
        ADSScript.enabled = false;
    }

    public void SetFPSCam(Transform newCam, GameObject newCamObject) {
        fpsCam = newCam;
        fpsCamObject = newCamObject;
    }

    public void SetEquip (bool equippedState) {
        equipped = equippedState;
    }

    public void SetGunContainer(GameObject newContainer) {
        gunContainer = newContainer;
    }

    private void SetLayerofChildren(Transform parent, string layer) {
        parent.gameObject.layer = LayerMask.NameToLayer(layer);
        if(parent.transform.childCount > 0) {
            foreach (Transform child in parent) {
                SetLayerofChildren(child, layer);
            }
        }
    } 
}
