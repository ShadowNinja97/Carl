using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class GunAbstract : MonoBehaviour
{
    [Header("Stats")]
    public int damage;
    public int magazineSize, burstSize, range, maxAmmo, startingAmmo;
    public float timeBetweenShooting, /*spread,*/ reloadTime, timeBetweenShots;
    public bool automatic;
    protected string weaponName;
    protected int bulletsLeft, bulletsShot, currentAmmo;

    // Bools
    protected bool shooting, readyToShoot, reloading;

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

    protected GameObject recoilCamContainer;
    protected Recoil cameraRecoil;

    [Header("Recoil Stats")]
    public float recoilX;
    public float recoilY;
    public float recoilZ;

    public float ADSRecoilX;
    public float ADSRecoilY;
    public float ADSRecoilZ;

    protected float timeFromLastShot;

    protected GameObject player;
    protected AimDownSights ADSscript;

    private void Awake() {
        currentAmmo = startingAmmo;
        bulletsLeft = magazineSize;
        readyToShoot = true;

        weaponName = gameObject.GetComponent<PickUpController>().itemName;

        ADSscript = gameObject.GetComponent<AimDownSights>();
    }

    protected abstract void MyInput();

    protected abstract void Reload();

    protected abstract void ReloadFinished();
    
    protected abstract void Shoot();

    protected void ResetShot() {
        readyToShoot = true;
    }

    public abstract int GetCurrentAmmo();

    public abstract void AddAmmo(int ammo);

    public void SetGunCam(GameObject newRecoilCamContainer, Camera newfpsCam, GameObject playerObj) {
        recoilCamContainer = newRecoilCamContainer;
        fpsCam = newfpsCam;
        player = playerObj;
        cameraRecoil = recoilCamContainer.GetComponent<Recoil>();
    }

    public GameObject GetRecoilCamContainer() {
        return recoilCamContainer;
    }

    public Camera GetfpsCam() {
        return fpsCam;
    }

    public GameObject GetPlayer() {
        return player;
    }
    
}
