using UnityEditor;
using UnityEngine;

public abstract class PlayerAbility : MonoBehaviour
{
    protected Player owner;

    enum aibilityNumber 
    {
        Normal,
        Animal,
    }


    public virtual void OnAbilityCopied(Player owner)
    {
        this.owner = owner;
    }

    public virtual void OnAbilityDestroyed(Player owner)
    {
        owner.GetComponentInChildren<Animator>().runtimeAnimatorController = 
            Resources.Load<RuntimeAnimatorController>("Test/Kirby");
        this.owner = null;         
       
    }

    public abstract void AttackHandle();
}
