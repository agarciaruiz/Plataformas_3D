using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSupply : MonoBehaviour, ISupply
{
    private GameObject player;
    private CharacterStats characterStats;
    private int shieldToAdd = 30;

    private void Start()
    {
        GetReferences();
    }

    public void PickupSupply()
    {
        if (characterStats.shield == characterStats.maxShield)
        {
            Debug.Log("Shield already full");
        }
        else
        {
            characterStats.AddShield(shieldToAdd);
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
