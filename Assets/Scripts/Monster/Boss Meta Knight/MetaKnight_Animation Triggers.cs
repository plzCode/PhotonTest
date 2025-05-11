using Photon.Pun;
using UnityEngine;

public class MetaKnight_AnimationTriggers : MonoBehaviour
{
    private BossMetaKnight boss => GetComponentInParent<BossMetaKnight>();


    private void AnimationTrigger()
    {
        boss.AnimationFinishTrigger();
    }



    private void SetCameraShake()
    {
        CameraShake.Instance.Shake(0.3f, 1.33f, 1.33f);
    }

    private void Attack1Effect()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            boss.photonView.RPC(
                "Attack1RPC",
                RpcTarget.All
            );
        }
    }

    private void Attack2Effect()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            boss.photonView.RPC(
                "Attack2RPC",
                RpcTarget.All
            );
        }
    }

    private void Attack3Effect()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            boss.photonView.RPC(
                "Attack3RPC",
                RpcTarget.All
            );
        }
    }

    private void AttackTrigger()
    {
        boss.SetVelocity(0 * boss.facingDir, 0);

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

    private void MyPosAttackTrigger()
    {
        boss.SetVelocity(0 * boss.facingDir, 0);

        if (!PhotonNetwork.IsMasterClient)
            return;



        Collider2D[] colliders = Physics2D.OverlapCircleAll(boss.attack1Check.position, boss.attack1CheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                if (hit.GetComponent<PhotonView>() != null)
                    hit.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, (Vector2)transform.position, boss.attackPower); // ������ ó��                


            }
        }
    }

    private void MyPosAirAttackTrigger()
    {

        if (!PhotonNetwork.IsMasterClient)
            return;



        Collider2D[] colliders = Physics2D.OverlapCircleAll(boss.attack1Check.position, boss.attack1CheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                if (hit.GetComponent<PhotonView>() != null)
                    hit.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, (Vector2)transform.position, boss.attackPower); // ������ ó��                


            }
        }
    }

    private void MyPosAttack4Trigger()
    {
        boss.SetVelocity(0 * boss.facingDir, 0);

        if (!PhotonNetwork.IsMasterClient)
            return;



        Collider2D[] colliders = Physics2D.OverlapCircleAll(boss.attack1Check.position, boss.attack1CheckRadius + 0.5f);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                if (hit.GetComponent<PhotonView>() != null)
                    hit.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, (Vector2)transform.position, boss.attackPower); // ������ ó��                


            }
        }
    }

    private void Jump()
    {
        boss.SetVelocity(4 * boss.facingDir, 10f);
    }

    private void AirJump()
    {
        boss.SetVelocity(4 * boss.facingDir, 6f);
    }

    private void AirStop()
    {
        Rigidbody2D rb = boss.GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0f; // �߷µ� ����
    }

    private void AirStopEnd()
    {
        Rigidbody2D rb = boss.GetComponent<Rigidbody2D>();
        rb.gravityScale = 1f; // �߷� ����
    }

    private void SetJumpCheck()
    {
        boss.isJump = true;
    }

    private void SetJumpCheckClose()
    {
        boss.isJump = false;
    }

    private void Setmove()
    {
        boss.attack4State.bossDash = true;
    }

    private void SetmoveClose()
    {
        boss.attack4State.bossDash = false;
    }
}
