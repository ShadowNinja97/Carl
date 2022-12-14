/* Notes:
Tutorial also included camera shake, but I wasn't sure if it fit with this style of game, so I left it out.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunBase : MonoBehaviour
{
    [Header("References")]
    public int damage;
    public int magazineSize, burstSize, range, maxAmmo, startingAmmo;
    public float timeBetweenShooting, /*spread,*/ reloadTime, timeBetweenShots;
    public bool automatic;
    string weaponName;
    int bulletsLeft, bulletsShot, currentAmmo;

    // Bools
    bool shooting, readyToShoot, reloading;

    [Header("References")]
    public Camera fpsCam;
    public Transform attackPoint, ejectionPort;
    public RaycastHit rayHit;
    public LayerMask enemyLayer;

    [Header("Graphics")]
    public ParticleSystem muzzleFlash;
    public GameObject bulletHoleGraphic;
    public TextMeshProUGUI bulletCounter;
    public TextMeshProUGUI gunName;
    public ParticleSystem shellSystem;

    [Header("Recoil")]
    private GameObject recoilCamContainer;
    private Recoil cameraRecoil;

    public float recoilX;
    public float recoilY;
    public float recoilZ;

    public float ADSRecoilX;
    public float ADSRecoilY;
    public float ADSRecoilZ;

    private float timeFromLastShot;

    private GameObject player;
    private AimDownSights ADSscript;

    private void Awake() {
        currentAmmo = startingAmmo;
        bulletsLeft = magazineSize;
        readyToShoot = true;

        weaponName = gameObject.GetComponent<PickUpController>().itemName;

        ADSscript = gameObject.GetComponent<AimDownSights>();
    }

    private void Update() {
        MyInput();

        // Set Name
        gunName.SetText(weaponName);

        // Set Bullet Counter
        bulletCounter.SetText(bulletsLeft + "|" + currentAmmo);
    }

    private void FixedUpdate() {
        timeFromLastShot += 0.02f;
    }

    private void MyInput() {
        if (automatic) {
            shooting = Input.GetKey(KeyBinds.shoot);
        }

        else {
            shooting = Input.GetKeyDown(KeyBinds.shoot);
        }

        if (Input.GetKeyDown(KeyBinds.reload) && bulletsLeft < magazineSize && currentAmmo > 0 && !reloading) {
            Reload();
        }

        //Shoot
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0 && timeFromLastShot >= timeBetweenShooting) {
            bulletsShot = burstSize;
            Shoot();
            timeFromLastShot = 0.0f;
        }
    }

    private void Reload() {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished() {
        if (currentAmmo > (magazineSize - bulletsLeft)) {
            currentAmmo -= (magazineSize - bulletsLeft);
            bulletsLeft = magazineSize;
        }
        else {
            bulletsLeft += currentAmmo;
            currentAmmo = 0;
        }
        reloading = false;
    }

    private void Shoot() {
        readyToShoot = false;

        // Bullet Spread
        /*float xSpread = Random.Range(-spread, spread);
        float ySpread = Random.Range(-spread, spread);
        float zSpread = Random.Range(-spread, spread); */
        // If we want to add more spread as the player moves, just multiply spread if velocity > 0.

        // Calculate Direction with Spread
        // Vector3 direction = fpsCam.transform.forward + new Vector3(xSpread, ySpread, zSpread);

        // RayCast
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out rayHit, range, enemyLayer)) {
            if (rayHit.collider.CompareTag("Enemy")) {
                // Gets the enemy script if the shot hit an enemy and performs a method to take damage. Enemy to be implemented.
                // rayHit.collider.GetComponent<nameOfEnemyScript>().enemyTakeDamageMethod(damage);
                Debug.Log("Hit enemy for " + damage + " damage.");
            }
        }

        // Graphics
        
        Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.LookRotation(rayHit.normal));
        // Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
        muzzleFlash.Emit(1);
        shellSystem.Emit(1);

        cameraRecoil.RecoilFire();

        bulletsLeft--;
        bulletsShot--;

        Invoke("ResetShot", timeBetweenShots);

        if (bulletsShot > 0 && bulletsLeft > 0) {
            Invoke("Shoot", timeBetweenShots);
        }
    }

    private void ResetShot() {
        readyToShoot = true;
    }

    public int getCurrentAmmo() {
        return currentAmmo + bulletsLeft;
    }

    public void addAmmo(int ammo) {
        if (currentAmmo + ammo < maxAmmo) {
            currentAmmo+=ammo;
        }
        else {
            currentAmmo = maxAmmo;
        }
    }

    public void setGunCam(GameObject newRecoilCamContainer, Camera newfpsCam, GameObject playerObj) {
        recoilCamContainer = newRecoilCamContainer;
        fpsCam = newfpsCam;
        player = playerObj;
        cameraRecoil = recoilCamContainer.GetComponent<Recoil>();
    }

    public GameObject getRecoilCamContainer() {
        return recoilCamContainer;
    }

    public Camera getfpsCam() {
        return fpsCam;
    }

    public GameObject getPlayer() {
        return player;
    }
}
