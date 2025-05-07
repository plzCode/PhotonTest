using UnityEngine;

public class Cat_AnimationTriggers : MonoBehaviour
{
    private Monster_Cat enemy => GetComponentInParent<Monster_Cat>();
    private void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();
    }

    private void ChangeMoveDown()
    {
        enemy.TurnMoveDown();
    }
}
