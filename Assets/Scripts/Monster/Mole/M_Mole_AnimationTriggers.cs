using UnityEngine;

public class M_Mole_AnimationTriggers : MonoBehaviour
{
    private Monster_Mole enemy => GetComponentInParent<Monster_Mole>();
    

    private void Mole_StartJump()
    {
        enemy.SetVelocity(enemy.facingDir *  6f, 4f);
        //Debug.Log("점프 시작");
    }

}
