using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    //[SerializeField] GameObject explosionParticles;
    [SerializeField] private int damage;
    //private LootableObj lootableObj;
    //private EnemyUI enemyUI;
    public float attackSpeed = 1f;

    private void Start()
    {
        GetReferences();
        InitVariables();
    }

    public void DealDamage(CharacterStats statsToDamage)
    {
        if (statsToDamage != null)
        {
            statsToDamage.TakeDamage(damage);
        }
    }

    public override void CheckHealth()
    {
        base.CheckHealth();
        //enemyUI.UpdateHealth(health, maxHealth);
    }

    public override void Die()
    {
        base.Die();
        //GameManager.enemiesLeft--;
        //Debug.Log(GameManager.enemiesLeft);
        //lootableObj.DropLoot();
        //GameObject explosion = Instantiate(explosionParticles, transform.position, transform.rotation);
        //PlayDieSound(explosion);
        Destroy(this.gameObject);
    }

    /*private void PlayDieSound(GameObject gameObject)
    {
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.Play();
    }*/

    public override void InitVariables()
    {
        SetHealthTo(maxHealth);
        isDead = false;
    }

    private void GetReferences()
    {
        //enemyUI = GetComponent<EnemyUI>();
        //lootableObj = GetComponent<LootableObj>();
    }
}
