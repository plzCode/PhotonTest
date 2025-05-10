using Photon.Pun;
using UnityEngine;

public class DDD_AnimationTriggers : MonoBehaviour
{
    private Boss_DDD boss => GetComponentInParent<Boss_DDD>();

    private void ISGround()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        if (boss.IsGroundDetected())
        {
            boss.photonView.RPC("ChangeState", RpcTarget.All, "Idle");
        }
        else
        {
            boss.photonView.RPC("ChangeState", RpcTarget.All, "Jump");
        }
    }

    private void Ground()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        if (boss.IsGroundDetected())
        {
            boss.photonView.RPC("ChangeState", RpcTarget.All, "Idle");
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


    [PunRPC]
    private void AirOutRPC()
    {

        if (PhotonNetwork.IsMasterClient)
        {
            //Debug.Log("ThrowSpear RPC ȣ��� - IsMasterClient: " + PhotonNetwork.IsMasterClient);
            GameObject _currentBomb = PhotonNetwork.Instantiate("Monster_Effect/" + boss.AirPrefab.name, boss.AirPos.position, Quaternion.identity);
            //Rigidbody2D _BombRidig = _currentBomb.GetComponent<Rigidbody2D>();
            //_BombRidig.linearVelocity = new Vector2(10 * facingDir, 10);
        }
    }


    public void CallAttack3RPC()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            boss.photonView.RPC("Attack3RPC", RpcTarget.All);
        }
    }


    #region Audio

    [PunRPC]
    private void Move__Sound()
    {
        AudioManager.Instance.RPC_PlaySFX("Boss_Move");
    }

    [PunRPC]
    private void Jump__Sound()
    {
        AudioManager.Instance.RPC_PlaySFX("BKS_Throw");
    }

    [PunRPC]
    private void Jump1__Sound()
    {
        AudioManager.Instance.RPC_PlaySFX("BKS_Jump");
    }

    [PunRPC]
    private void AirOut__Sound()
    {
        AudioManager.Instance.RPC_PlaySFX("Air_Out_Sound");
    }

    [PunRPC]
    private void Attack__Sound()
    {
        AudioManager.Instance.RPC_PlaySFX("DDD_Attack");
    }

    [PunRPC]
    private void Attack3__Sound()
    {
        AudioManager.Instance.RPC_PlaySFX("DDD_Attack3");
    }


    #endregion
}
