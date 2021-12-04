using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStats : CharacterStats
{
    //private EnemyUI enemyUI;
    //public float attackSpeed = 2f;

    private void Start()
    {
        GetReferences();
        InitVariables();
    }

    public void DealDamage(CharacterStats statsToDamage, int damage)
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
    }

    /*private void PlayDieSound(GameObject gameObject)
    {
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.Play();
    }*/

    public void DestroyEnemy()
    {
        Destroy(this.gameObject);
    }

    public override void InitVariables()
    {
        maxHealth = 1000;
        SetHealthTo(maxHealth);
        isDead = false;
    }

    private void GetReferences()
    {
        //enemyUI = GetComponent<EnemyUI>();
    }
}
