using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("UI")]
    public Slider healthBarSlider;
    public TextMeshProUGUI healthBarValueText;

    [Header("Death Settings")]
    [Tooltip("Scripts to disable when the player dies.")]
    public MonoBehaviour[] scriptsToDisable;

    [Header("Ability Hooks")]
    public bool isInvincible = false;

    [Tooltip("When true, player reflects damage to nearby enemies on hit.")]
    public bool thornsActive = false;
    public float thornsRadius = 4f;
    public float thornsDamage = 10f;

    private bool isDead = false;

    // Global flag enemies check
    public static bool PlayerIsDead { get; private set; }

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    // ---------------- REGEN ----------------
    [Header("Regen Settings")]
    [Tooltip("How long player must be still before regen can start.")]
    public float regenIdleSeconds = 3f;

    [Tooltip("Regen cannot start until this many seconds after taking damage.")]
    public float regenAfterHitDelay = 3f;

    [Tooltip("How far the player must move to count as 'moving'.")]
    public float movementThreshold = 0.02f;

    [Tooltip("How fast health increases during regen (HP per second).")]
    public float regenRate = 20f;

    private Vector3 lastPos;
    private float idleTimer = 0f;

    private float lastHitTime = -999f;

    private Coroutine regenRoutine;
    private bool regenUsedThisIdle = false; // only once per idle period (same behavior you already had)

    void Awake()
    {
        PlayerIsDead = false;
        isDead = false;
        lastPos = transform.position;
    }

    void Start()
    {
        currentHealth = maxHealth;

        if (healthBarSlider != null)
        {
            healthBarSlider.minValue = 0f;
            healthBarSlider.maxValue = maxHealth;
            healthBarSlider.value = currentHealth;
        }

        UpdateHealthUI();
    }

    void Update()
    {
        if (isDead) return;

        // Track movement by position change
        float movedDist = Vector3.Distance(transform.position, lastPos);
        lastPos = transform.position;

        bool isMoving = movedDist > movementThreshold;

        if (isMoving)
        {
            // moving cancels regen and resets idle tracking
            idleTimer = 0f;
            regenUsedThisIdle = false;
            StopRegen();
            return;
        }

        // Not moving
        idleTimer += Time.deltaTime;

        // must be idle long enough AND must be 3 seconds since last hit
        bool hitDelayPassed = Time.time >= lastHitTime + regenAfterHitDelay;

        if (!regenUsedThisIdle && idleTimer >= regenIdleSeconds && hitDelayPassed)
        {
            float target = GetRegenTarget();
            if (target > currentHealth + 0.01f)
            {
                StartRegenTo(target);
                regenUsedThisIdle = true; // once per idle period
            }
        }
    }

    // 31-59 => up to 60, 1-29 => up to 30, otherwise no regen
    private float GetRegenTarget()
    {
        if (currentHealth <= 0f) return 0f;

        if (currentHealth >= 31f && currentHealth <= 59f)
            return Mathf.Min(60f, maxHealth);

        if (currentHealth >= 1f && currentHealth <= 29f)
            return Mathf.Min(30f, maxHealth);

        return 0f;
    }

    private void StartRegenTo(float target)
    {
        StopRegen();
        regenRoutine = StartCoroutine(RegenRoutine(target));
    }

    private IEnumerator RegenRoutine(float target)
    {
        // Gradually increase health until target, but stop immediately if hit or dead
        while (!isDead && currentHealth < target - 0.01f)
        {
            currentHealth = Mathf.Min(target, currentHealth + regenRate * Time.deltaTime);
            UpdateHealthUI();
            yield return null;
        }

        regenRoutine = null;
    }

    private void StopRegen()
    {
        if (regenRoutine != null)
        {
            StopCoroutine(regenRoutine);
            regenRoutine = null;
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead || isInvincible) return;

        // record hit time FIRST so regen is delayed even if standing still
        lastHitTime = Time.time;

        // getting hit cancels regen instantly
        StopRegen();

        currentHealth = Mathf.Clamp(currentHealth - amount, 0f, maxHealth);
        UpdateHealthUI();

        if (thornsActive && amount > 0f)
            DoThorns();

        if (currentHealth <= 0f)
            Die();
    }

    public void Heal(float amount)
    {
        if (isDead) return;

        currentHealth = Mathf.Clamp(currentHealth + amount, 0f, maxHealth);
        UpdateHealthUI();
    }

    public void RestoreFullHealth()
    {
        if (isDead) return;
        StopRegen();
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (healthBarSlider != null)
            healthBarSlider.value = currentHealth;

        if (healthBarValueText != null)
            healthBarValueText.text = $"{Mathf.RoundToInt(currentHealth)}/{Mathf.RoundToInt(maxHealth)}";
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        PlayerIsDead = true;

        StopRegen();

        Debug.Log("Player has died!");

        foreach (var script in scriptsToDisable)
        {
            if (script != null)
                script.enabled = false;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        var rb = GetComponent<Rigidbody>();
        if (rb)
        {
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

    }

    public void ResetPlayer()
    {
        isDead = false;
        PlayerIsDead = false;

        StopRegen();

        idleTimer = 0f;
        regenUsedThisIdle = false;
        lastHitTime = -999f;
        lastPos = transform.position;

        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    // ---------------- THORNS ----------------
    private void DoThorns()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, thornsRadius);

        foreach (var hit in hits)
        {
            EnemyHealth eh = hit.GetComponentInParent<EnemyHealth>();
            if (eh != null && eh.CurrentHealth > 0f)
            {
                eh.TakeDamage(thornsDamage);
            }
        }
    }
}
