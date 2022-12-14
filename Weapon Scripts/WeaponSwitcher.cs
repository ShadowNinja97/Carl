using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    public int selectedWeaponIndex = 0;
    public GameObject mainCamera;
    public KeyCode switchWeaponKey = KeyCode.Alpha1;

    GameObject selectedWeapon;
    GameObject holsteredWeapon;

    // Start is called before the first frame update
    void Start()
    {
        selectedWeapon = mainCamera.GetComponent<CameraPrompt>().heldObject;
        holsteredWeapon = null;
    }

    // Update is called once per frame
    void Update()
    {
        // 3+ Weapons
        /* int previousSelectedWeaponIndex = selectedWeaponIndex;
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            selectedWeaponIndex = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount==2) {
            selectedWeaponIndex = 1;
        }

        if (selectedWeaponIndex!=previousSelectedWeaponIndex) {
            SelectWeapon();
        } */

        // 2 Weapons
        if (Input.GetKeyDown(switchWeaponKey)) {
            if (gameObject.transform.childCount > 1) {
                SelectWeapon();
            }
        }
    }

    void SelectWeapon() {
        
        // Useful for 3+ weapons
        /* int i = 0;
        foreach (Transform weapon in transform) {
            if(i == selectedWeaponIndex) {
                selectedWeapon = weapon.gameObject;
                weapon.gameObject.GetComponent<PickUpController>().PickUpItem();
                weapon.gameObject.GetComponent<PickUpController>().SetEquip(true);
                weapon.gameObject.SetActive(true);
            }
            else {
                weapon.gameObject.GetComponent<PickUpController>().SetEquip(false);
                weapon.gameObject.SetActive(false);
            }
            i++;
        } */

        // For 2 weapon limit
        foreach (Transform weapon in transform) {
            if(weapon.gameObject.GetComponent<PickUpController>().equipped) {
                holsteredWeapon = weapon.gameObject;
                weapon.gameObject.GetComponent<PickUpController>().SetEquip(false);
                weapon.gameObject.SetActive(false);
            }
            else {
                selectedWeapon = weapon.gameObject;
                mainCamera.GetComponent<CameraPrompt>().setHeldObject(selectedWeapon);
                weapon.gameObject.GetComponent<PickUpController>().PickUpItem();
                weapon.gameObject.GetComponent<PickUpController>().SetEquip(true);
                weapon.gameObject.SetActive(true);
            }
        }
    }

    public GameObject getSelectedWeapon() {
        return selectedWeapon;
    }

    public GameObject getHolsteredWeapon() {
        if (holsteredWeapon != null) {
            return holsteredWeapon;
        }
        else {
            return null;
        }
    }

    public void SetSelectedWeapon(GameObject newSelectedWeapon) {
        selectedWeapon = newSelectedWeapon;
    }

    public void SetHolsteredWeapon(GameObject newHolsteredWeapon) {
        holsteredWeapon = newHolsteredWeapon;
    }
    
}