using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem; // [NEW INPUT]

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn")]
    public GameObject enemyPrefab;
    public Transform player;
    public int spawnCountPerPress = 3;
    public float spawnRadius = 10f;
    public float yOffset = 0.1f;
    public int maxAlive = 50;

    [Header("NavMesh")]
    public float sampleMaxDistance = 5f;
    public int areaMask = NavMesh.AllAreas;

    // (Legacy overrides removed – they referenced fields that no longer exist)
    // public float overrideLifetime = -1f;
    // public float overrideDeathAnimLen = -1f;
    // public float overrideFadeDuration = -1f;

    [Header("Lifetime (optional – not used by EnemyScript now)")]
    public float lifetime = -1f;  // -1 = infinite (kept here in case you want to use it externally)

    // runtime
    private readonly List<GameObject> alive = new List<GameObject>();

    void Awake()
    {
        // Auto-find player by tag if not set
        if (player == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) player = p.transform;
        }
    }

    void Update()
    {
        // Press R to spawn (keyboard) or gamepad Y
        if ((Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame) ||
            (Gamepad.current != null && Gamepad.current.buttonNorth.wasPressedThisFrame))
        {
            SpawnBatch();
        }

        // Cleanup null entries (destroyed enemies)
        for (int i = alive.Count - 1; i >= 0; i--)
        {
            if (alive[i] == null) alive.RemoveAt(i);
        }
    }

    private void SpawnBatch()
    {
        if (!enemyPrefab)
        {
            Debug.LogWarning("EnemySpawner: No enemyPrefab assigned.");
            return;
        }
        if (!player)
        {
            Debug.LogWarning("EnemySpawner: No player assigned.");
            return;
        }

        int toSpawn = spawnCountPerPress;

        // Enforce cap
        if (maxAlive > 0)
        {
            int room = maxAlive - alive.Count;
            if (room <= 0)
            {
                Debug.Log("EnemySpawner: At maxAlive, not spawning more.");
                return;
            }
            toSpawn = Mathf.Min(toSpawn, room);
        }

        int spawned = 0;
        int attempts = 0;
        int maxAttempts = toSpawn * 8;

        while (spawned < toSpawn && attempts < maxAttempts)
        {
            attempts++;

            // pick a ring around the player
            Vector3 random = Random.insideUnitSphere;
            random.y = 0f;
            Vector3 tryPos = player.position + random.normalized * Random.Range(spawnRadius * 0.5f, spawnRadius);

            if (NavMesh.SamplePosition(tryPos, out NavMeshHit hit, sampleMaxDistance, areaMask))
            {
                Vector3 spawnPos = hit.position + Vector3.up * yOffset;
                Quaternion rot = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

                var go = Instantiate(enemyPrefab, spawnPos, rot);

                // Wire up EnemyScript (assign player target!)
                var es = go.GetComponent<EnemyScript>();
                if (es != null)
                {
                    es.player = player; // <-- important so the enemy knows who to chase/attack
                }

                alive.Add(go);
                spawned++;
            }
        }

        if (spawned > 0)
            Debug.Log($"EnemySpawner: Spawned {spawned} enemies.");
        else
            Debug.LogWarning("EnemySpawner: Failed to find NavMesh positions to spawn.");
    }
}