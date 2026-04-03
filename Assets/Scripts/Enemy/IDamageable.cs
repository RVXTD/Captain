public interface IDamageable
{
    void TakeDamage(float amount);
    float CurrentHealth { get; }
    float MaxHealth { get; }
}