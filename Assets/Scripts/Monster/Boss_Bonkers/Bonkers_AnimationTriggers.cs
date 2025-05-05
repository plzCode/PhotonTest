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
        //Debug.Log("CalledFunction ȣ���");
        // RPC ȣ���� ������ Ŭ���̾�Ʈ������ ó��
        if (PhotonNetwork.IsMasterClient)
        {
            // ���� ȣ���� �����, RPC�� ��ȯ�մϴ�.
            boss.photonView.RPC(
                "ThrowBombRPC",
                RpcTarget.All  // �Ǵ� RpcTarget.All
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
