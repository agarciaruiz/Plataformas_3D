using UnityEngine;

public class MedSupply : MonoBehaviour ,ISupply
{
    private GameObject player;
    private CharacterStats characterStats;
    private int healthToAdd = 30;

    private void Start()
    {
        GetReferences();
    }

    public void PickupSupply()
    {
        if (characterStats.health == characterStats.maxHealth)
        {
            Debug.Log("Healt already full");
        }
        else
        {
            characterStats.Heal(healthToAdd);
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PickupSupply();
    }

    private void GetReferences()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        characterStats = player.GetComponent<CharacterStats>();
    }
}
