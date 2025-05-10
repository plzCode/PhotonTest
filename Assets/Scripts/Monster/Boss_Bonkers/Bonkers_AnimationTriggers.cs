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

    
    private void SetCameraShake()
    {
        CameraShake.Instance.Shake(0.3f, 1.33f, 1.33f);
    }

    private void AttackTrigger()
    {
        SetCameraShake();

        if (!PhotonNetwork.IsMasterClient)
            return;



        Collider2D[] colliders = Physics2D.OverlapCircleAll(boss.attackCheck.position, boss.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                if (hit.GetComponent<PhotonView>() != null)
                    hit.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, (Vector2)transform.position, boss.attackPower); // 데미지 처리                

                
            }
        }
    }

    private void CalledFunction()
    {
        boss.rb.AddForce(Vector2.up * 15, ForceMode2D.Impulse);

    }

    private void ForwardJump()
    {

        boss.SetVelocity(4 * boss.facingDir, 15);


    }

    private void SmallBackJump()
    {
        boss.rb.linearVelocity = new Vector2(3 * -boss.facingDir, 10);
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


    #region Audio

    [PunRPC]
    private void Move__Sound()
    {
        AudioManager.Instance.RPC_PlaySFX("Boss_Move");
    }

    [PunRPC]
    private void Attack__Sound()
    {
        AudioManager.Instance.RPC_PlaySFX("BKS_Attack");
    }

    [PunRPC]
    private void Attack1__Sound()
    {
        AudioManager.Instance.RPC_PlaySFX("BKS_Attack1");
    }

    [PunRPC]
    private void Jump__Sound()
    {
        AudioManager.Instance.RPC_PlaySFX("BKS_Jump");
    }

    [PunRPC]
    private void JumpEnd__Sound()
    {
        AudioManager.Instance.RPC_PlaySFX("BKS_JumpEnd");
    }

    [PunRPC]
    private void Throw__Sound()
    {
        AudioManager.Instance.RPC_PlaySFX("BKS_Throw");
    }
    #endregion
}
