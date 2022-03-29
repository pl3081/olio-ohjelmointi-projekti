public interface IAttackingObject
{
    int AttackDamage { get; }
    float AttackRange { get; }
    float AttackSpeed { get; }
    bool Attack();
}
