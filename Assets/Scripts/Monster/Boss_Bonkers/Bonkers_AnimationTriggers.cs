using Photon.Pun;
using UnityEngine;

public class Bonkers_AnimationTriggers : MonoBehaviour
{
    private Boss_Bonkers boss => GetComponentInParent<Boss_Bonkers>();


    private void AnimationTrigger()
    {
        boss.AnimationFinishTrigger();
    }

    private void CalledFunction()
    {        
        boss.rb.AddForce(Vector2.up * 20, ForceMode2D.Impulse);        
        
    }

    private void ForwardJump()
    {

        boss.SetVelocity(5*boss.facingDir, 20);


    }
    private void SmallJump()
    {
        
            boss.SetVelocity(0, 15);
        
        
    }

    private void SetJumpCheck()
    {
        boss.isJump = true;
    }

    
}
