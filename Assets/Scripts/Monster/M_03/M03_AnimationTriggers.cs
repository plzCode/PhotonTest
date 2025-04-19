using UnityEngine;

public class M03_AnimationTriggers : MonoBehaviour
{
    

    private Monster_03 enemy => GetComponentInParent<Monster_03>();
    private void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();
    }

    
}
