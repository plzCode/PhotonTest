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
                    hit.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, (Vector2)transform.position, boss.attackPower); // ������ ó��                

                
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


}
