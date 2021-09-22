namespace Assets.Scripts.Interfaces
{
    public interface ICombatCapable
    {
        void Attack(CharacterBase defendingChar);
        void Die();
        void TakeDamage();

    }
}

