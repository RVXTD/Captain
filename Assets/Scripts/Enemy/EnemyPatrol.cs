using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol Settings")]
    public float patrolRadius = 15f;        // how far from start to wander
    public float walkSpeed = 1.8f;
    public float pauseTime = 2f;            // pause between points

    private UnityEngine.AI.NavMeshAgent agent;
    private Animator anim;
    private Vector3 nextPoint;
    private float pauseTimer;

    void Awake()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        agent.speed = walkSpeed;
        SetNextDestination();
    }

    void Update()
    {
        // Update animation blend
        anim.SetFloat("MoveSpeed", agent.velocity.magnitude);

        // Check if we reached the point
        if (!agent.pathPending && agent.remainingDistance < 0.4f)
        {
            pauseTimer += Time.deltaTime;
            if (pauseTimer >= pauseTime)
            {
                SetNextDestination();
                pauseTimer = 0f;
            }
        }
    }

    void SetNextDestination()
    {
        // pick a random NavMesh position near current location
        for (int i = 0; i < 10; i++)
        {
            Vector3 random = Random.insideUnitSphere * patrolRadius + transform.position;
            if (UnityEngine.AI.NavMesh.SamplePosition(random, out UnityEngine.AI.NavMeshHit hit, 2f, UnityEngine.AI.NavMesh.AllAreas))
            {
                nextPoint = hit.position;
                agent.SetDestination(nextPoint);
                return;
            }
        }
    }

    // visual debug in Scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, patrolRadius);
    }
}
