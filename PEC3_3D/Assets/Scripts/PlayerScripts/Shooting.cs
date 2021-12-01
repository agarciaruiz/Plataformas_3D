using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;

public class Shooting : MonoBehaviour
{
    [SerializeField] private Rig aimLayer;
    [SerializeField] private float aimDuration = 0.1f;
    [SerializeField] private Transform target;
    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private ParticleSystem hitEffect;

    private Inventory inventory;
    private EquipmentManager equipmentManager;
    private PlayerHUD playerHUD;
    private Animator animator;
    //private AudioSource gunAudio;
    //[SerializeField] AudioClip reloadClip;

    private PlayerInput playerInput;
    private InputAction reloadAction;

    private float lastFireTime;
    private bool canShoot;
    private bool canReload;
    private bool isAiming;

    [HideInInspector] public int primaryCurrentAmmo;
    [HideInInspector] public int primaryCurrentAmmoStorage;
    private bool primaryMagazineIsEmpty = false;

    [HideInInspector] public int secondaryCurrentAmmo;
    [HideInInspector] public int secondaryCurrentAmmoStorage;
    private bool secondaryMagazineIsEmpty = false;

    private void Awake()
    {
        GetReferences();
        InitVariables();
    }

    void Update()
    {
        if (Mouse.current.rightButton.isPressed)
        {
            StartAim();
        }

        if (Mouse.current.rightButton.wasReleasedThisFrame)
        {
            CancelAim();
        }

        if (Mouse.current.leftButton.isPressed)
        {
            Shoot();
        }

        if (reloadAction.triggered)
        {
            Reload(equipmentManager.currentlyEquipedWeapon);
            /*gunAudio.clip = reloadClip;
            gunAudio.Play();*/
        }
    }

    private void RaycastShoot(Weapon currentWeapon)
    {
        Ray ray = new Ray();
        RaycastHit hit;

        float currentWeaponRange = currentWeapon.range;
        Transform barrel = equipmentManager.currentWeaponBarrel;

        ray.origin = barrel.position;
        ray.direction = target.position - barrel.position;

        if (Physics.Raycast(ray, out hit, currentWeaponRange))
        {
            Debug.Log(hit.transform.name);
            if (hit.collider.gameObject.tag == "Enemy")
            {
                /*CharacterStats enemyStats = hit.collider.gameObject.GetComponentInParent<CharacterStats>();
                EnemyAI enemyAI = hit.collider.gameObject.GetComponentInParent<EnemyAI>();
                enemyAI.isHit = true;
                DealDamage(enemyStats, currentWeapon.damage);*/
            }

            if (hit.collider.gameObject.tag != "Enemy")
            {
                hitEffect.transform.position = hit.point;
                hitEffect.transform.forward = hit.normal;
                hitEffect.Emit(1);
            }
        }
        muzzleFlash.Emit(1);
    }

    private void Shoot()
    {
        Weapon currentWeapon = inventory.GetItem(equipmentManager.currentlyEquipedWeapon);

        CheckCanShoot(equipmentManager.currentlyEquipedWeapon);
        //InitAudio(currentWeapon);

        if (canShoot && canReload)
        {
            if (Time.time > lastFireTime + currentWeapon.fireRate)
            {
                lastFireTime = Time.time;
                RaycastShoot(currentWeapon);
                UseAmmo((int)currentWeapon.weaponStyle, 1, 0);
                StartCoroutine(ShootAnim());
                /*gunAudio.clip = currentWeapon.fireFX;
                gunAudio.Play();*/
            }
        }
    }

    private void UseAmmo(int slot, int currentAmmoUsed, int currentStoredAmmoUsed)
    {
        if(slot == 0)
        {
            if(primaryCurrentAmmo <= 0)
            {
                primaryMagazineIsEmpty = true;
                CheckCanShoot(equipmentManager.currentlyEquipedWeapon);
            }
            else
            {
                primaryCurrentAmmo -= currentAmmoUsed;
                primaryCurrentAmmoStorage -= currentStoredAmmoUsed;
                playerHUD.UpdateAmmoUI(primaryCurrentAmmo, primaryCurrentAmmoStorage);
            }
        }

        if(slot == 1)
        {
            if(secondaryCurrentAmmo <= 0)
            {
                secondaryMagazineIsEmpty = true;
                CheckCanShoot(equipmentManager.currentlyEquipedWeapon);
            }
            else
            {
                secondaryCurrentAmmo -= currentAmmoUsed;
                secondaryCurrentAmmoStorage -= currentStoredAmmoUsed;
                playerHUD.UpdateAmmoUI(secondaryCurrentAmmo, secondaryCurrentAmmoStorage);
            }
        }
    }

    public void AddAmmo(int slot, int currentAmmoAdded, int currentStoredAmmoAdded)
    {
        if (slot == 0)
        {
            primaryCurrentAmmo += currentAmmoAdded;
            primaryCurrentAmmoStorage += currentStoredAmmoAdded;
            playerHUD.UpdateAmmoUI(primaryCurrentAmmo, primaryCurrentAmmoStorage);
        }

        if (slot == 1)
        {
            secondaryCurrentAmmo += currentAmmoAdded;
            secondaryCurrentAmmoStorage += currentStoredAmmoAdded;
            playerHUD.UpdateAmmoUI(secondaryCurrentAmmo, secondaryCurrentAmmoStorage);
        }
    }

    private void Reload(int slot)
    {
        if (canReload)
        {
            if (slot == 0)
            {
                int ammoToReload = inventory.GetItem(slot).magazineSize - primaryCurrentAmmo;

                if (primaryCurrentAmmoStorage >= ammoToReload)
                {
                    if (primaryCurrentAmmo == inventory.GetItem(slot).magazineSize)
                    {
                        canReload = false;
                        Debug.Log("Magazine full");
                    }
                    else
                        canReload = true;

                    AddAmmo(slot, ammoToReload, 0);
                    UseAmmo(slot, 0, ammoToReload);

                    primaryMagazineIsEmpty = false;
                    CheckCanShoot(slot);
                }
                else
                    Debug.Log("Not enough ammo to reaload");
            }

            if (slot == 1)
            {
                int ammoToReload = inventory.GetItem(slot).magazineSize - secondaryCurrentAmmo;

                if (secondaryCurrentAmmoStorage >= ammoToReload)
                {
                    if (secondaryCurrentAmmo == inventory.GetItem(slot).magazineSize)
                    {
                        canReload = false;
                        Debug.Log("Magazine full");
                    }
                    else
                        canReload = true;

                    AddAmmo(slot, ammoToReload, 0);
                    UseAmmo(slot, 0, ammoToReload);

                    secondaryMagazineIsEmpty = false;
                    CheckCanShoot(slot);
                }
                else
                    Debug.Log("Not enough ammo to reaload");
            }
        }
        else
            Debug.Log("Can't reload now");
    }

    private void StartAim()
    {
        isAiming = true;
        aimLayer.weight += Time.deltaTime / aimDuration;
    }

    private void CancelAim()
    {
        isAiming = false;
        aimLayer.weight -= Time.deltaTime / aimDuration;
    }

    /*public void DealDamage(CharacterStats statsToDamage, int damage)
    {
        statsToDamage.TakeDamage(damage);
    }*/

    IEnumerator ShootAnim()
    {
        animator.SetLayerWeight(animator.GetLayerIndex("Shoot Layer"), 1);
        animator.SetTrigger("Shoot");
        yield return new WaitForSeconds(0.9f);
        animator.SetLayerWeight(animator.GetLayerIndex("Shoot Layer"), 0);
    }

    private void CheckCanShoot(int slot)
    {
        if(slot == 0)
        {
            if (primaryMagazineIsEmpty)
                canShoot = false;
            else
                canShoot = true;
        }

        if(slot == 1)
        {
            if (secondaryMagazineIsEmpty)
                canShoot = false;
            else
                canShoot = true;
        }
    }

    public void InitAmmo(int slot, Weapon weapon)
    {
        if(slot == 0)
        {
            primaryCurrentAmmo = weapon.magazineSize;
            primaryCurrentAmmoStorage = weapon.storedAmmo;
        }

        if(slot == 1)
        {
            secondaryCurrentAmmo = weapon.magazineSize;
            secondaryCurrentAmmoStorage = weapon.storedAmmo;
        }
    }

    /*private void InitAudio(Weapon weapon)
    {
        //gunAudio.clip = weapon.fireFX;
    }*/

    private void GetReferences()
    {
        inventory = GetComponent<Inventory>();
        equipmentManager = GetComponent<EquipmentManager>();
        playerInput = GetComponent<PlayerInput>();
        playerHUD = GetComponent<PlayerHUD>();
        animator = GetComponent<Animator>();
        //gunAudio = GetComponent<AudioSource>();
    }

    private void InitVariables()
    {
        reloadAction = playerInput.actions["Reload"];
        canShoot = true;
        canReload = true;
        isAiming = false;
    }
}
