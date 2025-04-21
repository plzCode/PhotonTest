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

    public abstract void AttackHandle();
}
