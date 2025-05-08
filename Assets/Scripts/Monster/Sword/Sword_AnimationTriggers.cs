using Photon.Pun;
using UnityEngine;

public class Sword_AnimationTriggers : MonoBehaviour
{

    private Monster_Sword enemy => GetComponentInParent<Monster_Sword>();
    private void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();
    }

    private void AttackForwardStart()
    {
        enemy.Sword_AttackForward();
    }
    private void AttackForwardStop()
    {
        enemy.SetZeroVelocity();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                if (hit.GetComponent<PhotonView>() != null)
                {
                    Vector2 tmpPos = new Vector2(transform.position.x, transform.position.y);
                    Debug.Log(hit.name);
                    hit.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, tmpPos, enemy.attackPower); // 데미지 처리
                }

                Debug.Log("스워드 몬스터가 플레이어에게 " + enemy.attackPower + "만큼 데미지를 줌");
            }
        }
    }
}
