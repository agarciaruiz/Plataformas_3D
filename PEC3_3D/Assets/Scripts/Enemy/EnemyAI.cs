using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private ParticleSystem bloodParticles;
    [HideInInspector] public WanderState wanderState;
    [HideInInspector] public AlertState alertState ;
    [HideInInspector] public AttackState attackState ;
    [HideInInspector] public IEnemyState currentState;

    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public Animator animator;
    [HideInInspector] public GameObject target;

    [HideInInspector] public EnemyStats enemyStats;

    [HideInInspector] public float rotationTime = 10.0f;
    [HideInInspector] public float shootHeight = 0.5f;
    [HideInInspector] public bool isHit;
    [HideInInspector] public bool scream = false;


    void Start()
    {
        GetReferences();
        InitVariables();
    }

    void Update()
    {
        if (!enemyStats.isDead)
        {
            currentState.UpdateState();

            if (isHit)
            {
                currentState.Impact();
                isHit = false;
            }

            if (scream)
            {
                StartCoroutine(ScreamAnimation());
                scream = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        currentState.OnTriggerEnter(other);
    }

    private void OnTriggerStay(Collider other)
    {
        currentState.OnTriggerStay(other);
    }

    private void OnTriggerExit(Collider other)
    {
        currentState.OnTriggerExit(other);
    }

    public void FollowTarget()
    {
        attackState.FollowTarget();
    }

    public void DamageTarget()
    {
        attackState.DamageTarget();
    }

    public IEnumerator ScreamAnimation()
    {
        animator.SetTrigger("Scream");
        yield return new WaitForSeconds(2.5f);
        currentState.ToAttackState();
    }

    public void EnableBlood()
    {
        bloodParticles.Play();
    }

    public void DisableBlood()
    {
        bloodParticles.Stop();
    }

    private void InitVariables()
    {
        isHit = false;
        currentState = wanderState;
    }

    private void GetReferences()
    {
        wanderState = new WanderState(this);
        alertState = new AlertState(this);
        attackState = new AttackState(this);

        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player");
        enemyStats = GetComponent<EnemyStats>();
    }
}
