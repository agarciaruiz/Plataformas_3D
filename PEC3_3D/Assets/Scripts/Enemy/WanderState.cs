using UnityEngine;

public class WanderState : IEnemyState
{
    EnemyAI enemyAI;
    private int nextWayPoint = 0;
    private float wanderRadius = 100;

    public WanderState(EnemyAI enemy)
    {
        enemyAI = enemy;
    }

    public void UpdateState()
    {
        Vector3 randPos = Random.insideUnitSphere * wanderRadius;

        if (enemyAI.navMeshAgent != null && enemyAI.navMeshAgent.remainingDistance <= enemyAI.navMeshAgent.stoppingDistance)
        {
            enemyAI.navMeshAgent.destination = randPos;
        }

        /*enemyAI.navMeshAgent.destination = enemyAI.wayPoints[nextWayPoint].position;

        if (!enemyAI.navMeshAgent.pathPending && enemyAI.navMeshAgent.remainingDistance < 0.5f)
        {
            nextWayPoint = (nextWayPoint + 1) % enemyAI.wayPoints.Length;
        }*/
    }

    public void Impact()
    {
        ToAttackState();
    }

    public void ToAlertState()
    {
        Debug.Log("Alert State");
        enemyAI.navMeshAgent.isStopped = true;    
        enemyAI.currentState = enemyAI.alertState;
    }


    public void ToAttackState()
    {
        enemyAI.navMeshAgent.isStopped = true;
        enemyAI.shouldFollow = true;
        enemyAI.currentState = enemyAI.attackState;
    }

    public void ToWanderState() {}

    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            ToAlertState();
        }
    }

    public void OnTriggerStay(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            ToAlertState();
        }
    }

    public void OnTriggerExit(Collider col) {}
}
