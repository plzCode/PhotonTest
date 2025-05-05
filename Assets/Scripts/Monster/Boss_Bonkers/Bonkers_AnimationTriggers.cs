using Photon.Pun;
using UnityEngine;

public class Bonkers_AnimationTriggers : MonoBehaviour
{
    private Boss_Bonkers boss => GetComponentInParent<Boss_Bonkers>();


    private void AnimationTrigger()
    {
        boss.AnimationFinishTrigger();
    }

    private void ThrowBomb()
    {
        //Debug.Log("CalledFunction 호출됨");
        // RPC 호출을 마스터 클라이언트에서만 처리
        if (PhotonNetwork.IsMasterClient)
        {
            // 직접 호출을 지우고, RPC로 전환합니다.
            boss.photonView.RPC(
                "ThrowBombRPC",
                RpcTarget.All  // 또는 RpcTarget.All
            );
        }
    }

    private void CalledFunction()
    {        
        boss.rb.AddForce(Vector2.up * 20, ForceMode2D.Impulse);        
        
    }

    private void ForwardJump()
    {

        boss.SetVelocity(5*boss.facingDir, 20);


    }

    private void SmallBackJump()
    {
        boss.rb.linearVelocity = new Vector2(5 * -boss.facingDir, 10);
    }

    private void SmallJump()
    {        
          boss.SetVelocity(0, 15);      
    }

    private void SetJumpCheck()
    {
        boss.isJump = true;
    }

    private void SetBackMoveCancel()
    {
        boss.attackState.isBackWalking = false;
    }

    private void SetBackMove()
    {
        boss.attackState.isBackWalking = true;
    }

    
}
