using UnityEngine;

public class Ability_Animal : PlayerAbility
{
    public RuntimeAnimatorController animalKirby;
    
    public override void OnAbilityCopied(Player owner)
    {
        base.OnAbilityCopied(owner);
        animalKirby = animalKirby = Resources.Load<RuntimeAnimatorController>("Test/Animal_Kirby");
        owner.GetComponentInChildren<Animator>().runtimeAnimatorController = animalKirby;
        Debug.Log("Animal ability copied");
    }

    public override void AttackHandle()
    {
        Debug.Log("Animal ability attack");
    }
}
