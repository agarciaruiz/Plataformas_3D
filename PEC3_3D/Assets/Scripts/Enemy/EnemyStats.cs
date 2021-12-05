    using UnityEngine;

public class EnemyStats : CharacterStats
{
    [SerializeField] private int damage;
    private LootableObj lootableObj;
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

    public override void Die()
    {
        base.Die();
        GetComponent<Animator>().SetTrigger("Die");
    }

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
        lootableObj = GetComponent<LootableObj>();
    }
}
