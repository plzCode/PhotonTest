using UnityEngine;

public class Waddle_AnimationTriggers : MonoBehaviour
{
    

    private Monster_Waddle enemy => GetComponentInParent<Monster_Waddle>();
    private void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();
    }

    
}
