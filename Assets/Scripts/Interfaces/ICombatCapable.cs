namespace Assets.Scripts.Interfaces
{
    public interface ICombatCapable
    {
        void Attack(ICombatCapable defendingChar);
        void Die(ICombatCapable attacker);
        void TakeDamage(ICombatCapable attacker, int damage, bool isFatal);

    }
}

