using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Pistol : GunAbstract
{

    void Update()
    {
        MyInput();

        // Set Name
        gunName.SetText(weaponName);

        // Set Bullet Counter
        bulletCounter.SetText(bulletsLeft + "|" + currentAmmo);
    }

    void FixedUpdate() {
        timeFromLastShot += 0.02f;
    }

    protected override void MyInput() {
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

    protected override void Reload() {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    protected override void ReloadFinished() {
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
    
    protected override void Shoot() {
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

    public override int GetCurrentAmmo() {
        return currentAmmo + bulletsLeft;
    }

    public override void AddAmmo(int ammo) {
        if (currentAmmo + ammo < maxAmmo) {
            currentAmmo+=ammo;
        }
        else {
            currentAmmo = maxAmmo;
        }
    }
}
