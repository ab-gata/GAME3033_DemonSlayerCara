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
    [SerializeField] protected WeaponHolder weaponHolder;

    public bool isFiring;
    public bool isRealoding;

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

    protected virtual void FireWeapon()
    {
        weaponStats.bulletsInClip--;
        Debug.Log("FIRING WEAPON Bullets in clip : " + weaponStats.bulletsInClip);
    }

    public virtual void StartReloading()
    {
        isRealoding = true;
        ReloadWeapon();
    }

    public virtual void StopReloading()
    {
        isRealoding = false;
    }

    protected virtual void ReloadWeapon()
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
