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
}
