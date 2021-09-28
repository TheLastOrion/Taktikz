namespace Assets.Scripts.Interfaces
{
    public interface ICombatCapable
    {
        void Attack(ICombatCapable defendingChar);
        void Die();
        void TakeDamage();

    }
}

