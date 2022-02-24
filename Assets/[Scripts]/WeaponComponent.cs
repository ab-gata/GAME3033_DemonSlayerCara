using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WeaponStats
{
    // Weapon attributes
    public string weaponName;
    public float damage;

    public int clipSize;
    public int bulletsInClip;
    public int totalBullets;

    public float fireStartDelay;
    public float fireRate;
    public float fireDistance;
    public bool repeating;
    public LayerMask hitLayers;
}

public class WeaponComponent : MonoBehaviour
{
    // Weapon attributes
    public Transform gripLocation;
    public WeaponStats weaponStats;
    protected WeaponHolder weaponHolder;

    public bool isFiring;
    public bool isReloading;

    // Weapon Effects
    [SerializeField] protected ParticleSystem firingEffect;

    // Depending components
    protected Camera mainCamera;


    void Awake()
    {
        mainCamera = Camera.main;
    }

    public void Initialize(WeaponHolder _weaponHolder)
    {
        weaponHolder = _weaponHolder;
    }

    public virtual void StartFiringWeapon()
    {
        isFiring = true;
        if (weaponStats.repeating)
        {
            // fireeee
            InvokeRepeating(nameof(FireWeapon), weaponStats.fireStartDelay, weaponStats.fireRate);
        }
        else
        {
            FireWeapon();
        }
    }

    public virtual void StopFiringWeapon()
    {
        isFiring = false;
        CancelInvoke(nameof(FireWeapon));

        if (firingEffect && firingEffect.isPlaying)
        {
            firingEffect.Stop();
        }
    }

    public void FireWeapon()
    {
        Vector3 hitLocation;

        if (weaponStats.bulletsInClip > 0 && !isReloading && !weaponHolder.Player.Sprinting)
        {
            if (firingEffect)
            {
                firingEffect.Play();
            }

            weaponStats.bulletsInClip--;
            Debug.Log("FIRING WEAPON Bullets in clip : " + weaponStats.bulletsInClip);

            Ray screenRay = mainCamera.ViewportPointToRay(new Vector2(0.5f, 0.5f));

            if (Physics.Raycast(screenRay, out RaycastHit hit, weaponStats.fireDistance, weaponStats.hitLayers))
            {
                hitLocation = hit.point;
                Vector3 hitDirection = hit.point - mainCamera.transform.position;
                Debug.DrawRay(mainCamera.transform.position, hitDirection.normalized * weaponStats.fireDistance, Color.red, 1);
            }
        }
        else if (weaponStats.bulletsInClip <= 0)
        {
            // trigger reload if no bullets left
            weaponHolder.StartReloading();
        }
    }

    public void StartReloading()
    {
        isReloading = true;
        ReloadWeapon();
    }

    public void StopReloading()
    {
        isReloading = false;
    }

    public void ReloadWeapon()
    {
        // stop firing effect if there is one playing
        if (firingEffect && firingEffect.isPlaying)
        {
            firingEffect.Stop();
        }

        int bulletsToReload = weaponStats.clipSize - weaponStats.totalBullets;

        if (bulletsToReload < 0)
        {
            weaponStats.bulletsInClip = weaponStats.clipSize;
            weaponStats.totalBullets -= weaponStats.clipSize;
        }
        else
        {
            weaponStats.bulletsInClip = weaponStats.totalBullets;
            weaponStats.totalBullets = 0;
        }
    }
}
