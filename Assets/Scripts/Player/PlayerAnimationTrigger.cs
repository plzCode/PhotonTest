using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Linq;

public class PlayerAnimatorController : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();


    private void AnimationTrigger()
    {
        player.AnimationFinishTrigger();
    }

    public void DowningGround_Idle_State()
    {
        if (!player.IsGroundCheck() && !player.isSlope)
        {
            player.stateMachine.ChangeState(player.airState);
        }
        else
            player.stateMachine.ChangeState(player.idleState);
        //에니메이터에서 idle로 가게함
    }

    public void Idle_Air_state()
    {
        if (!player.IsGroundCheck() && !player.isSlope)
        {
            player.stateMachine.ChangeState(player.airState);
        }
        else
            player.stateMachine.ChangeState(player.idleState);
    }

    public void ChangeForm()
    {

        AudioManager.Instance.RPC_PlaySFX("Nomikomi_Sound");
        player.KirbyFormNum = player.EatKirbyFormNum; //저장되어있던 몹 넘버를 변신하는 넘버로 넘긴다
        player.GetComponent<PhotonView>().RPC("SyncFormNum", RpcTarget.AllBuffered, player.EatKirbyFormNum);

        if (player.KirbyFormNum > 0) //0이상이면 먹은 적 커비로 변신
        {
            //player.KirbyFrom(); //변신
            player.Call_RPC("KirbyForm", RpcTarget.All); //변신
            player.stateMachine.ChangeState(player.changeFormState);
        }
        else
        {
            player.curAbility.OnAbilityDestroyed(player); //0이면 기본 커비로 변환
            Destroy(player.curAbility);
            player.stateMachine.ChangeState(player.idleState);
            Debug.Log("Change Form");
        }
    }

    public void AttackTrigger()
    {
        if(player.curAbility == null || !player.pView.IsMine) return; //어빌리티가 없으면 리턴
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.curAbility.attackCheckRadius);
        if(AudioManager.Instance != null && player.curAbility.SFX_Name != "")
        {
            AudioManager.Instance.RPC_PlaySFX(player.curAbility.SFX_Name);
        }
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Enemy>().photonView.RPC("TakeDamage", RpcTarget.All,player.curAbility.attackPower); // 데미지 처리
                //hit.gameObject.SetActive(false);  //임시로
            }
            else if(hit.GetComponent<StarBlock>() != null)
            {
                hit.GetComponent<StarBlock>().pv.RPC("Delete", RpcTarget.All); //블록 제거
            }
            else if (hit.GetComponent<BigStarBlock>() != null)
            {
                hit.GetComponent<BigStarBlock>().pv.RPC("Delete", RpcTarget.All); //블록 제거
            }
        }
        player.curAbility.SFX_Name = ""; //어빌리티의 SFX 이름 초기화
    }
    public void DownAttackTrigger()
    {
        if (player.curAbility == null || !player.pView.IsMine) return; //어빌리티가 없으면 리턴        

        if (AudioManager.Instance != null && player.curAbility.SFX_Name != "")
        {
            //AudioManager.Instance.PlaySFX(player.curAbility.SFX_Name);
            AudioManager.Instance.RPC_PlaySFX(player.curAbility.SFX_Name);
        }
        StartCoroutine(CheckOverlapForSeconds(()=> new Vector2(player.transform.position.x, player.transform.position.y - 1f), player.curAbility.attackCheckRadius, 0.3f)); //공격 범위 체크
        player.curAbility.SFX_Name = ""; //어빌리티의 SFX 이름 초기화
    }
    public void DashAttackTrigger()
    {
        if (player.curAbility == null || !player.pView.IsMine) return; //어빌리티가 없으면 리턴
        if (AudioManager.Instance != null && player.curAbility.SFX_Name != "")
        {
            //AudioManager.Instance.PlaySFX(player.curAbility.SFX_Name);
            AudioManager.Instance.RPC_PlaySFX(player.curAbility.SFX_Name);
        }
        StartCoroutine(CheckOverlapForSeconds(()=>player.attackCheck.position, player.curAbility.attackCheckRadius, 0.3f)); //공격 범위 체크
        player.curAbility.SFX_Name = ""; //어빌리티의 SFX 이름 초기화
    }



    [PunRPC]
    public void EatKirbyStarAttack()
    {
        RangedAttack(Attack, "Player_Effect/Kirby Eat Attack 60x60_0");
    }

    [PunRPC]
    public void CutterKirbyAttack()
    {
        RangedAttack(Attack, "Player_Effect/Cutter");
    }




    [PunRPC]
    public void SwordKirbyAttack()
    {
        RangedAttack(Attack, "Player_Effect/Attack2 Effect_0");
    }



    [PunRPC]
    public void WhellKirbyAttackEffect()
    {
        
        RangedAttack(Attack, "Player_Effect/Whell Attack Effect_0");
    }


    private GameObject Attack;

    private void RangedAttack(GameObject rangeAttack, string rangeAttackName)
    {
        if (!player.pView.IsMine) return;
        if (rangeAttack == null)
        {
            rangeAttack = Resources.Load<GameObject>(rangeAttackName); //원거리 공격을 가져옴
        }

        //player.EffectAdd(player.LastMove, rangeAttack, player.AirJumpOutEffectPos); //플레이어 한태서 공격 발사
        player.pView.RPC("EffectForCutter", RpcTarget.All, player.LastMove, rangeAttack.name, player.AirJumpOutEffectPos.position); //플레이어 한태서 공격 발사
        Debug.Log(rangeAttack.name);
    }

    //Animal Kirby Dash Attack
    IEnumerator CheckOverlapForSeconds(Func<Vector2> getCenter, float radius, float duration)
    {
        float timer = 0f;
        HashSet<int> processedIDs = new HashSet<int>();

        while (timer < duration)
        {
            Vector2 center = getCenter();
            Collider2D[] colliders = Physics2D.OverlapCircleAll(center, radius);

            foreach (var col in colliders)
            {
                int id = col.GetInstanceID();  // collider 기준 ID
                if (processedIDs.Contains(id)) continue;

                if (col.TryGetComponent<Enemy>(out var enemy))
                {
                    enemy.photonView.RPC("TakeDamage", RpcTarget.All, player.curAbility.attackPower);
                }
                else if (col.TryGetComponent<StarBlock>(out var starBlock))
                {
                    starBlock.pv.RPC("Delete", RpcTarget.All);
                }
                else if (col.TryGetComponent<BigStarBlock>(out var bigBlock))
                {
                    bigBlock.pv.RPC("Delete", RpcTarget.All);
                }

                processedIDs.Add(id);
            }

            timer += Time.deltaTime;
            yield return null;
        }
    }





    #region Audio

    [PunRPC]
    public void Animal_1_Sound()
    {
        AudioManager.Instance.RPC_PlaySFX("kirby_Animal_1");
    }





    [PunRPC]
    public void Sword_1_Sound()
    {
        AudioManager.Instance.RPC_PlaySFX("kirby_Sword_1");
    }

    [PunRPC]
    public void Sword_2_Sound()
    {
        AudioManager.Instance.RPC_PlaySFX("kirby_Sword_2");
    }

    [PunRPC]
    public void Sword_3_Sound()
    {
        AudioManager.Instance.RPC_PlaySFX("kirby_Sword_3");
    }

    [PunRPC]
    public void Sword_4_Sound()
    {
        AudioManager.Instance.RPC_PlaySFX("kirby_Sword_4");
    }



    #endregion
}
