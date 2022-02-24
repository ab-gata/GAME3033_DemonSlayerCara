using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponHolder : MonoBehaviour
{
    [SerializeField]
    GameObject weaponToSpawn;

    public Sprite crossHairImage;

    [SerializeField]
    GameObject weaponSocketLocation;
    [SerializeField]
    Transform gripIKSocketLocation;

    public Sprite crosshairImage;

    WeaponComponent weaponComponent;
    public PlayerBehaviour player;
    Animator playerAnimator;

    bool firingPressed = false;

    public readonly int isFiringHash = Animator.StringToHash("IsFiring");
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

    public void OnFire(InputValue value)
    {
        firingPressed = value.isPressed;
        if (firingPressed)
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

        playerAnimator.SetBool(isFiringHash, true);
        player.Shooting = true;
        weaponComponent.StartFiringWeapon();
    }

    void StopFiring()
    {
        playerAnimator.SetBool(isFiringHash, false);
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
        if (player.Shooting)
        {
            StopFiring();
        }

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

        if (firingPressed)
        {
            StartFiring();
        }
    }
}
