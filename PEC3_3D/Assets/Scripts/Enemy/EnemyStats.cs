using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStats : CharacterStats
{
    [SerializeField] private int damage;
    private LootableObj lootableObj;
    //private EnemyUI enemyUI;
    public float attackSpeed = 2f;

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
        GetComponent<Animator>().SetTrigger("Die");
        //GameManager.enemiesLeft--;
        //Debug.Log(GameManager.enemiesLeft);
    }

    /*private void PlayDieSound(GameObject gameObject)
    {
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.Play();
    }*/

    public void DestroyEnemy()
    {
        lootableObj.DropLoot();
        Destroy(this.gameObject);
    }

    public override void InitVariables()
    {
        SetHealthTo(maxHealth);
        isDead = false;
    }

    private void GetReferences()
    {
        //enemyUI = GetComponent<EnemyUI>();
        lootableObj = GetComponent<LootableObj>();
    }
}
