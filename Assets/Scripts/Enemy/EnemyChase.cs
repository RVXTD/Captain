using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    [Header("Target")]
    public Transform player;              // drag Sonic root here

    [Header("Sensing")]
    public float detectionRange = 12f;    // start chasing inside this
    public float loseSightMultiplier = 1.5f; // stop chasing when farther than detectionRange * this

    [Header("Speeds")]
    public float walkSpeed = 1.8f;        // patrol speed
    public float runSpeed = 3.6f;        // chase speed

    [Header("Patrol")]
    public float patrolRadius = 15f;      // wander radius
    public float pauseTime = 1.5f;        // stand briefly at points

    UnityEngine.AI.NavMeshAgent agent;
    Animator anim;
    Vector3 patrolPoint;
    float pauseT;
    bool chasing;

    static readonly int MoveSpeed = Animator.StringToHash("MoveSpeed");

    void Awake()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = GetComponentInChildren<Animator>(true);
        if (!anim) Debug.LogError("EnemyChase: No Animator found on enemy.");
    }

    void Start()
    {
        agent.speed = walkSpeed;
        SetNextPatrolPoint();
        agent.stoppingDistance = 1.5f;     // tweak as needed
    }

    void Update()
    {
        // 1) Drive the blend tree from actual velocity
        if (anim && anim.runtimeAnimatorController)
            anim.SetFloat(MoveSpeed, agent.velocity.magnitude);

        if (!player) { PatrolTick(); return; }

        float dist = Vector3.Distance(transform.position, player.position);

        // 2) State switch: patrol <-> chase
        if (!chasing && dist <= detectionRange)
        {
            chasing = true;
            agent.speed = runSpeed;
        }
        else if (chasing && dist > detectionRange * loseSightMultiplier)
        {
            chasing = false;
            agent.speed = walkSpeed;
            SetNextPatrolPoint();
        }

        // 3) Do the behavior
        if (chasing) ChaseTick();
        else PatrolTick();
    }

    void PatrolTick()
    {
        if (agent.destination != patrolPoint)
            agent.SetDestination(patrolPoint);

        if (!agent.pathPending && agent.remainingDistance < 0.4f)
        {
            pauseT += Time.deltaTime;
            if (pauseT >= pauseTime)
            {
                SetNextPatrolPoint();
                pauseT = 0f;
            }
        }
    }

    void ChaseTick()
    {
        agent.SetDestination(player.position);
    }

    void SetNextPatrolPoint()
    {
        for (int i = 0; i < 10; i++)
        {
            Vector3 random = transform.position + Random.insideUnitSphere * patrolRadius;
            if (UnityEngine.AI.NavMesh.SamplePosition(random, out var hit, 2f, UnityEngine.AI.NavMesh.AllAreas))
            {
                patrolPoint = hit.position;
                agent.SetDestination(patrolPoint);
                return;
            }
        }
        patrolPoint = transform.position;
    }

    // (optional) visualize ranges
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow; Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = new Color(1f, .5f, 0f); Gizmos.DrawWireSphere(transform.position, detectionRange * loseSightMultiplier);
    }
}