using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CameraPrompt : MonoBehaviour
{
    /* float mouseX = 0f;
    float mouseY = 0f;
    public float cameraSensitivity = 2f; */

    public GameObject player;

    Camera playerCam;
    private float FOV;

    public RaycastHit lookingPoint;

    public TextMeshProUGUI pickUpPrompt, weaponTextUI, ammoCounterUI;

    public GameObject heldObject, recoilCamHolder, defaultHeldObject, gunContainer;


    // Start is called before the first frame update
    void Start()
    {
        FOV = gameObject.GetComponent<Camera>().fieldOfView;   
    }

    // Update is called once per frame
    void Update()
    {
        // Used to control camera movement
        /* mouseX += Input.GetAxis("Mouse X") * cameraSensitivity;
        mouseY -= Input.GetAxis("Mouse Y") * cameraSensitivity;

        mouseY = Mathf.Clamp(mouseY, -90f, 90f);

        transform.localEulerAngles = new Vector3(mouseY, 0f, 0f);
        player.localEulerAngles = new Vector3(0f, mouseX, 0f); */
    }
    void FixedUpdate() {
        playerCam = gameObject.GetComponent<Camera>();
        if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out lookingPoint)) {
            if((lookingPoint.collider.CompareTag("Item") || lookingPoint.collider.CompareTag("Weapon")) && lookingPoint.distance <= player.GetComponent<PickUp>().pickUpRange && !lookingPoint.collider.GetComponent<PickUpController>().equipped) {
                if(heldObject != defaultHeldObject && lookingPoint.collider.GetComponent<PickUpController>().itemName.Equals(heldObject.GetComponent<PickUpController>().itemName)) {
                    pickUpPrompt.SetText("Press [" + KeyBinds.interact + "] to take " + lookingPoint.collider.GetComponent<PickUpController>().itemName + " ammo");
                }
                else if (gunContainer.GetComponent<WeaponSwitcher>().getHolsteredWeapon() != null && lookingPoint.collider.GetComponent<PickUpController>().itemName.Equals(gunContainer.GetComponent<WeaponSwitcher>().getHolsteredWeapon().GetComponent<PickUpController>().itemName)) {
                    pickUpPrompt.SetText("Press [" + KeyBinds.interact + "] to take " + lookingPoint.collider.GetComponent<PickUpController>().itemName + " ammo");
                }
                else {
                    pickUpPrompt.SetText("Press [" + KeyBinds.interact + "] to pick up " + lookingPoint.collider.GetComponent<PickUpController>().itemName);
                    if(Input.GetKeyDown(KeyCode.F)) {
                        lookingPoint.collider.GetComponent<PickUpController>().SetFPSCam(gameObject.transform, gameObject);
                        // lookingPoint.collider.GetComponent<GunBase>().setGunCam(recoilCamHolder, gameObject.GetComponent<Camera>());
                    }
                }
            }
            else {
                pickUpPrompt.SetText("");
            }
        }
        if (heldObject == defaultHeldObject) {
            weaponTextUI.SetText("[No Weapon]");
            ammoCounterUI.SetText("");
        }
    }

    public void setHeldObject(GameObject thisObject) {
        heldObject = thisObject;
    }

    public void setDefaultObject() {
        heldObject = defaultHeldObject;
    }

    public float getFOV() {
        return FOV;
    }
}
