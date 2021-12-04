using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IEnemyState
{
    EnemyAI enemyAI;
    private float lastAttackTime = 0;
    private bool hasReachedPlayer = false;

    public AttackState(EnemyAI enemy)
    {
        enemyAI = enemy;
    }

    public void UpdateState()
    {
        Vector3 lookDir = enemyAI.target.transform.position - enemyAI.transform.position;
        LookAtTarget(lookDir);
        enemyAI.animator.SetBool("Idle", false);
        if (!enemyAI.GetComponent<EnemyStats>().isDead)
        {
            FollowTarget();
        }
        else
        {
            enemyAI.navMeshAgent.isStopped = true;
        }

        if (enemyAI.navMeshAgent.remainingDistance <= enemyAI.navMeshAgent.stoppingDistance)
        {
            enemyAI.animator.SetBool("Idle", true);
            if (!hasReachedPlayer)
            {
                hasReachedPlayer = true;
                lastAttackTime = Time.time;
            }

            if (Time.time >= lastAttackTime + enemyAI.GetComponent<EnemyStats>().attackSpeed)
            {
                lastAttackTime = Time.time;
                AttackAnim();
            }
        }
        else
        {
            if (hasReachedPlayer)
            {
                hasReachedPlayer = false;
            }
        }
    }

    public void Impact() {}

    public void ToAlertState()
    {
        enemyAI.navMeshAgent.isStopped = true;
        enemyAI.currentState = enemyAI.alertState;
    }

    public void ToWanderState() { }
    public void ToAttackState() { }

    public void OnTriggerEnter(Collider col){}

    public void OnTriggerStay(Collider col){}

    public void OnTriggerExit(Collider col)
    {
        ToAlertState();
    }

    private void AttackAnim()
    {
        enemyAI.animator.SetTrigger("Attack");
    }

    public void DamageTarget()
    {
        if (!enemyAI.target.GetComponent<PlayerStats>().isDead)
        {
            enemyAI.enemyStats.DealDamage(enemyAI.target.GetComponent<PlayerStats>());
        }
    }

    public void FollowTarget()
    {
        enemyAI.navMeshAgent.isStopped = false;
        enemyAI.navMeshAgent.speed = 1.5f;
        enemyAI.navMeshAgent.SetDestination(enemyAI.target.transform.position);
    }
    
    private void LookAtTarget(Vector3 lookDir)
    {
        enemyAI.transform.rotation = Quaternion.FromToRotation(Vector3.forward, new Vector3(lookDir.x, 0, lookDir.z));
    }
}
