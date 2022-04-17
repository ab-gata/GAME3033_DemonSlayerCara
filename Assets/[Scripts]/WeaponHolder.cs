using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponHolder : MonoBehaviour
{
    // Weapon and hold
    [SerializeField] private GameObject weaponToSpawn;
    [SerializeField] private GameObject weaponSocketLocation;
    [SerializeField] private Transform gripIKSocketLocation;

    [SerializeField, Header("Raycasting")]
    private LayerMask layerMask;

    // Components
    private WeaponComponent weaponComponent;
    private PlayerBehaviour player;
    public PlayerBehaviour Player { get { return player; } }
    private Animator playerAnimator;

    bool shootingPressed = false;

    // Animation hashes
    public readonly int isShootingHash = Animator.StringToHash("IsShooting");
    public readonly int isReloadingHash = Animator.StringToHash("IsReloading");

    // Start is called before the first frame update
    void Start()
    {
        GameObject spawnedWeapon = Instantiate(weaponToSpawn,
                                               weaponSocketLocation.transform.position, weaponSocketLocation.transform.rotation,
                                               weaponSocketLocation.transform);

        player = GetComponent<PlayerBehaviour>();
        playerAnimator = GetComponent<Animator>();

        weaponComponent = spawnedWeapon.GetComponent<WeaponComponent>();
        weaponComponent.Initialize(this);
        PlayerEvents.InvokeOnWeaponEquipped(weaponComponent);
        gripIKSocketLocation = weaponComponent.gripLocation;
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (!player.Reloading)
        {
            playerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            playerAnimator.SetIKPosition(AvatarIKGoal.LeftHand, gripIKSocketLocation.transform.position);
        }
    }

    public void AddBulletDamage(int value)
    {
        weaponComponent.AddBulletDamage(value);
    }

    public void OnFire(InputValue value)
    {
        shootingPressed = value.isPressed;
        if (shootingPressed)
        {
            StartFiring();
        }
        else
        {
            StopFiring();
        }
    }

    void StartFiring()
    {
        if (weaponComponent.weaponStats.bulletsInClip <= 0)
        {
            StartReloading();
            return;
        }

        playerAnimator.SetBool(isShootingHash, true);
        player.Shooting = true;
        weaponComponent.StartFiringWeapon();
    }

    void StopFiring()
    {
        playerAnimator.SetBool(isShootingHash, false);
        player.Shooting = false;
        weaponComponent.StopFiringWeapon();
    }

    public void OnReload(InputValue value)
    {
        player.Reloading = value.isPressed;
        StartReloading();
    }

    public void StartReloading()
    {
        if (weaponComponent.weaponStats.totalBullets <= 0)
        {
            return;
        }

        weaponComponent.StartReloading();

        player.Reloading = true;
        playerAnimator.SetBool(isReloadingHash, true);
        // playerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);

        InvokeRepeating(nameof(StopReloading), 0, 0.1f);
    }

    public void StopReloading()
    {
        if (playerAnimator.GetBool(isReloadingHash)) return;

        player.Reloading = false;
        playerAnimator.SetBool(isReloadingHash, false);
        weaponComponent.StopReloading();
        CancelInvoke(nameof(StopReloading));

        if (shootingPressed)
        {
            StartFiring();
        }
    }

    public void OnShoot(InputValue value)
    {
         bool hit = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hitInfo, 20, layerMask);

         if (hit)
         {
             if (hitInfo.collider.CompareTag("Enemy"))
             {
                 Debug.Log("SHOOT");
                 weaponComponent.TryShoot(hitInfo.collider.GetComponent<EnemyBehaviour>());
             }

         }
    }
}
