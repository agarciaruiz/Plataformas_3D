using UnityEngine;

public class AlertState: IEnemyState
{
    EnemyAI enemyAI;
    float currentRotationTime = 0;

    public AlertState(EnemyAI enemy)
    {
        enemyAI = enemy;
    }

    public void UpdateState()
    {
        enemyAI.transform.rotation *= Quaternion.Euler(0f, Time.deltaTime * 360 * 1.0f / enemyAI.rotationTime, 0f);

        if(currentRotationTime > enemyAI.rotationTime)
        {
            currentRotationTime = 0;
            ToWanderState();
        }
        else
        {
            RaycastHit hit;

            if(Physics.Raycast(new Ray(
                new Vector3(enemyAI.transform.position.x, 0.5f, enemyAI.transform.position.z),
                enemyAI.transform.forward * 100),
                out hit))
            {
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    ToAttackState();
                }
            }

            currentRotationTime += Time.deltaTime;
        }
    }

    public void ToAlertState() {}
    public void ToAttackState()
    {
        enemyAI.shouldFollow = true;
        enemyAI.currentState = enemyAI.attackState;
    }

    public void ToWanderState()
    {
        enemyAI.navMeshAgent.isStopped = false;
        enemyAI.currentState = enemyAI.wanderState;
    }

    public void Impact()
    {
        ToAttackState();
    }

    public void OnTriggerEnter(Collider col) {}
    public void OnTriggerStay(Collider col) {}
    public void OnTriggerExit(Collider col) {}
}
