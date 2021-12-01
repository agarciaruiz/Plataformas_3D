using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    [HideInInspector] public WanderState wanderState;
    [HideInInspector] public AlertState alertState ;
    [HideInInspector] public AttackState attackState ;
    [HideInInspector] public IEnemyState currentState;

    [HideInInspector] public NavMeshAgent navMeshAgent;
    //[HideInInspector] public AudioSource fireAudio;
    [HideInInspector] public GameObject target;

    [HideInInspector] public EnemyStats enemyStats;

    [HideInInspector] public float rotationTime = 3.0f;
    [HideInInspector] public float shootHeight = 0.5f;
    [HideInInspector] public float walkSpeed = 0.1f;
    //public Transform[] wayPoints;
    [HideInInspector] public bool isHit;
    [HideInInspector] public bool shouldFollow;


    void Start()
    {
        GetReferences();
        InitVariables();
    }

    void Update()
    {
        currentState.UpdateState();
        enemyStats.CheckHealth();

        if (isHit)
        {
            currentState.Impact();
            isHit = false;
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

    private void InitVariables()
    {
        isHit = false;
        shouldFollow = false;
        navMeshAgent.speed = walkSpeed;
        currentState = wanderState;
    }

    private void GetReferences()
    {
        wanderState = new WanderState(this);
        alertState = new AlertState(this);
        attackState = new AttackState(this);

        navMeshAgent = GetComponent<NavMeshAgent>();
        //fireAudio = GetComponent<AudioSource>();
        target = GameObject.FindGameObjectWithTag("Player");
        enemyStats = GetComponent<EnemyStats>();
    }
}
