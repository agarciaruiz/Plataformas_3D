using UnityEngine;

public class AmmoSupply : MonoBehaviour, ISupply
{
    private GameObject player;
    private Shooting shooting;
    private EquipmentManager manager;
    private PlayerController playerController;
    private AudioSource audioSource;
    private Inventory inventory;
    private Weapon currentWeapon;

    private int ammoToAdd;
    private int storageToAdd;

    private void Start()
    {
        GetReferences();
    }

    public void PickupSupply()
    {
        currentWeapon = inventory.GetItem(manager.currentlyEquipedWeapon);

        if (currentWeapon.weaponStyle == WeaponStyle.Primary)
        {
            ammoToAdd = currentWeapon.magazineSize - shooting.primaryCurrentAmmo;
            storageToAdd = currentWeapon.storedAmmo - shooting.primaryCurrentAmmoStorage;
        }

        if (currentWeapon.weaponStyle == WeaponStyle.Secondary)
        {
            ammoToAdd = currentWeapon.magazineSize - shooting.secondaryCurrentAmmo;
            storageToAdd = currentWeapon.storedAmmo - shooting.secondaryCurrentAmmoStorage;
        }

        if (shooting.primaryCurrentAmmo == currentWeapon.magazineSize)
        {
            Debug.Log("Magazine full");
        }
        else
        {
            audioSource.clip = playerController.pickupClip;
            audioSource.Play();
            shooting.AddAmmo(manager.currentlyEquipedWeapon, ammoToAdd, storageToAdd);
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PickupSupply();
        }
    }

    private void GetReferences()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        shooting = player.GetComponent<Shooting>();
        manager = player.GetComponent<EquipmentManager>();
        inventory = player.GetComponent<Inventory>();
        playerController = player.GetComponent<PlayerController>();
        audioSource = player.GetComponent<AudioSource>();
    }
}
