using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;

    public float flashDuration = 0.1f;
    public Color flashColor = Color.red;

    private EnemyScript enemyScript;
    private Renderer[] renderers;
    private Color[] originalColors;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    void Awake()
    {
        enemyScript = GetComponent<EnemyScript>();
        renderers = GetComponentsInChildren<Renderer>();

        originalColors = new Color[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i].material.HasProperty("_Color"))
                originalColors[i] = renderers[i].material.color;
        }
    }

    void Start()
    {
        currentHealth = maxHealth;
        RoundManager.Instance?.RegisterEnemy();
    }

    public void TakeDamage(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
        StartCoroutine(FlashRed());

        if (currentHealth <= 0)
            Die();
    }

    private IEnumerator FlashRed()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i].material.HasProperty("_Color"))
                renderers[i].material.color = flashColor;
        }

        yield return new WaitForSeconds(flashDuration);

        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i].material.HasProperty("_Color"))
                renderers[i].material.color = originalColors[i];
        }
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
    }

    private void Die()
    {
        Debug.Log($"{name} has died!");

        RoundManager.Instance?.UnregisterEnemy();

        if (enemyScript != null)
            enemyScript.DoDeath();
        else
            Destroy(gameObject);
    }
}