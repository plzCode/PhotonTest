using UnityEngine;

public class Ability_Eat : PlayerAbility
{
    public RuntimeAnimatorController EatKirby;

    public override void OnAbilityCopied(Player owner)
    {
        base.OnAbilityCopied(owner);
        EatKirby = Resources.Load<RuntimeAnimatorController>("Test/Eat_Kirby");
        owner.GetComponentInChildren<Animator>().runtimeAnimatorController = EatKirby;
        Debug.Log("Animal ability copied");
    }

    public override void OnAbilityDestroyed(Player owner)
    {
        base.OnAbilityDestroyed(owner);
        Debug.Log("Animal ability destroyed");
    }
    public override void AttackHandle()
    {
        Debug.Log("Animal ability attack");
    }
}
