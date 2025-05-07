using UnityEngine;

public class Wheel_AnimationTriggers : MonoBehaviour
{
    private Monster_Wheel enemy => GetComponentInParent<Monster_Wheel>();
    private void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();
    }

    private void ChangeTurn()
    {
        enemy.WhellTurn();
    }
}
