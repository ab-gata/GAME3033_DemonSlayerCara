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

    private float timer;

    public bool isFiring;
    public bool isReloading;

    private GameController game;

    // Weapon Effects
    [SerializeField] protected ParticleSystem firingEffect;

    public void Initialize(WeaponHolder _weaponHolder)
    {
        weaponHolder = _weaponHolder;
    }

    private void Start()
    {
        game = FindObjectOfType<GameController>();
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
    }

    public void TryShoot(EnemyBehaviour eb)
    {
        if (timer <= 0)
        {
            eb.ShotAt(weaponStats.damage);
            weaponStats.bulletsInClip -= 1;
            timer = weaponStats.fireRate;
        }
    }

    public void AddBulletDamage(int value)
    {
        weaponStats.damage += value;

        game.UpdateBulletDamageHUD((int)weaponStats.damage);
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

    public virtual void StartFiringWeapon()
    {
        //isFiring = true;
        //if (weaponStats.repeating)
        //{
        //    // fireeee
        //    InvokeRepeating(nameof(FireWeapon), weaponStats.fireStartDelay, weaponStats.fireRate);
        //}
        //else
        //{
        //    FireWeapon();
        //}
    }

    public virtual void StopFiringWeapon()
    {
        //isFiring = false;
        //CancelInvoke(nameof(FireWeapon));

        //if (firingEffect && firingEffect.isPlaying)
        //{
        //    firingEffect.Stop();
        //}
    }

    public void FireWeapon()
    {
        //Vector3 hitLocation;

        //if (weaponStats.bulletsInClip > 0 && !isReloading && !weaponHolder.Player.Sprinting)
        //{
        //    if (firingEffect)
        //    {
        //        firingEffect.Play();
        //    }

        //    weaponStats.bulletsInClip--;
        //    Debug.Log("FIRING WEAPON Bullets in clip : " + weaponStats.bulletsInClip);

        //    Ray screenRay = mainCamera.ViewportPointToRay(new Vector2(0.5f, 0.5f));

        //    if (Physics.Raycast(screenRay, out RaycastHit hit, weaponStats.fireDistance, weaponStats.hitLayers))
        //    {
        //        hitLocation = hit.point;
        //        Vector3 hitDirection = hit.point - mainCamera.transform.position;
        //        Debug.DrawRay(mainCamera.transform.position, hitDirection.normalized * weaponStats.fireDistance, Color.red, 1);
        //    }
        //}
        //else if (weaponStats.bulletsInClip <= 0)
        //{
        //    // trigger reload if no bullets left
        //    weaponHolder.StartReloading();
        //}
    }
}
