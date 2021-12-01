using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IEnemyState
{
    EnemyAI enemyAI;
    private float lastAttackTime = 0;
    private float stoppingDistance = 5;

    public AttackState(EnemyAI enemy)
    {
        enemyAI = enemy;
    }

    public void UpdateState()
    {
        float distToTarget = Vector3.Distance(enemyAI.transform.position, enemyAI.target.transform.position);
        Vector3 lookDir = enemyAI.target.transform.position - enemyAI.transform.position;

        FollowTarget(distToTarget);
        LookAtTarget(lookDir);
    }

    public void Impact() {}

    public void ToAlertState()
    {
        enemyAI.currentState = enemyAI.alertState;
    }

    public void ToWanderState() { }
    public void ToAttackState() { }

    public void OnTriggerEnter(Collider col){}

    public void OnTriggerStay(Collider col)
    {
        if (col.GetComponent<CharacterStats>() != null && !col.GetComponent<CharacterStats>().isDead)
        {
            if (Time.time >= lastAttackTime + enemyAI.enemyStats.attackSpeed)
            {
                lastAttackTime = Time.time;
                Attack(col.GetComponent<CharacterStats>());
            }
        }
    }

    public void OnTriggerExit(Collider col)
    {
        enemyAI.shouldFollow = false;
        ToAlertState();
    }

    private void Attack(CharacterStats statsToDamage)
    {
        if (!statsToDamage.isDead)
        {
            //enemyAI.fireAudio.Play();
            enemyAI.enemyStats.DealDamage(statsToDamage);
        }
    }

    private void FollowTarget(float distance)
    {
        if (enemyAI.shouldFollow && distance >= stoppingDistance)
        {
            enemyAI.transform.position = Vector3.MoveTowards(enemyAI.transform.position, enemyAI.target.transform.position, enemyAI.walkSpeed * Time.deltaTime);
        }
    }

    private void LookAtTarget(Vector3 lookDir)
    {
        enemyAI.transform.rotation = Quaternion.FromToRotation(Vector3.forward, new Vector3(lookDir.x, 0, lookDir.z));
    }
}
