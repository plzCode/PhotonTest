using UnityEngine;

public class Sword_AnimationTriggers : MonoBehaviour
{

    private Monster_Sword enemy => GetComponentInParent<Monster_Sword>();
    private void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();
    }

    private void AttackForwardStart()
    {
        enemy.Sword_AttackForward();
    }
    private void AttackForwardStop()
    {
        enemy.SetZeroVelocity();
    }
}
