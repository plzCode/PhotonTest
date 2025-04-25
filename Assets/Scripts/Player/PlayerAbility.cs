using UnityEditor;
using UnityEngine;

public abstract class PlayerAbility : MonoBehaviour
{
    protected Player owner;

    enum AibilityNumber 
    {
        Normal, //0
        Eat, //1
        Animal, //2
    }


    public virtual void OnAbilityCopied(Player owner)
    {
        this.owner = owner;
    }

    public virtual void OnAbilityDestroyed(Player owner)
    {
        owner.GetComponentInChildren<Animator>().runtimeAnimatorController = 
            Resources.Load<RuntimeAnimatorController>("Test/Kirby");
        owner.KirbyFormNum = 0; //커비 변신 초기화
        this.owner = null;         
       
    }

    public abstract void AttackHandle();
}
