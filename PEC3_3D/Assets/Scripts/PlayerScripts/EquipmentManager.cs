using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EquipmentManager : MonoBehaviour
{
    public int currentlyEquipedWeapon = 0;
    public Transform currentWeaponBarrel = null;
    private GameObject currentWeapon = null;

    [SerializeField] private Transform weaponHolder = null;
    private Inventory inventory;

    [SerializeField] Weapon defaultWeapon = null;
    private PlayerHUD playerHUD;
    [SerializeField] private Shooting shooting;

    private PlayerInput playerInput;
    private InputAction equipPrimary;
    private InputAction equipSecondary;

    private void Start()
    {
        GetReferences();
        InitVariables();
    }

    private void Update()
    {
        if (equipPrimary.triggered && currentlyEquipedWeapon != 0)
        {
            UnequipWeapon();
            EquipWeapon(inventory.GetItem(0));
        }
        if (equipSecondary.triggered && currentlyEquipedWeapon != 1)
        {
            if (inventory.GetItem(1) != null)
            {
                UnequipWeapon();
                EquipWeapon(inventory.GetItem(1));
            }
        }
    }

    private void EquipWeapon(Weapon weapon)
    {
        playerHUD.UpdateWeaponUI(weapon);
        if((int)weapon.weaponStyle == 0)
        {
            playerHUD.UpdateAmmoUI(shooting.primaryCurrentAmmo, shooting.primaryCurrentAmmoStorage);
        }
        if((int)weapon.weaponStyle == 1)
        {
            playerHUD.UpdateAmmoUI(shooting.secondaryCurrentAmmo, shooting.secondaryCurrentAmmoStorage);
        }
        currentlyEquipedWeapon = (int)weapon.weaponStyle;
        currentWeapon = Instantiate(weapon.prefab, weaponHolder);
        currentWeaponBarrel = currentWeapon.transform.GetChild(0);
    }

    private void UnequipWeapon()
    {
        Destroy(currentWeapon);
    }

    private void InitVariables()
    {
        inventory.AddItem(defaultWeapon);
        EquipWeapon(inventory.GetItem(0));
        equipPrimary = playerInput.actions["EquipPrimary"];
        equipSecondary = playerInput.actions["EquipSecondary"];
    }

    private void GetReferences()
    {
        inventory = GetComponent<Inventory>();
        shooting = GetComponent<Shooting>();
        playerInput = GetComponent<PlayerInput>();
        playerHUD = GetComponent<PlayerHUD>();
    }
}
