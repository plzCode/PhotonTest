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
        //���ϸ����Ϳ��� idle�� ������
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
        player.KirbyFormNum = player.EatKirbyFormNum; //����Ǿ��ִ� �� �ѹ��� �����ϴ� �ѹ��� �ѱ��
        player.GetComponent<PhotonView>().RPC("SyncFormNum", RpcTarget.AllBuffered, player.EatKirbyFormNum);

        if (player.KirbyFormNum > 0) //0�̻��̸� ���� �� Ŀ��� ����
        {
            //player.KirbyFrom(); //����
            player.Call_RPC("KirbyForm", RpcTarget.All); //����
            player.stateMachine.ChangeState(player.changeFormState);
        }
        else
        {
            player.curAbility.OnAbilityDestroyed(player); //0�̸� �⺻ Ŀ��� ��ȯ
            Destroy(player.curAbility);
            player.stateMachine.ChangeState(player.idleState);
            Debug.Log("Change Form");
        }
    }

    public void AttackTrigger()
    {
        if(player.curAbility == null || !player.pView.IsMine) return; //�����Ƽ�� ������ ����
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.curAbility.attackCheckRadius);
        if(AudioManager.Instance != null && player.curAbility.SFX_Name != "")
        {
            AudioManager.Instance.RPC_PlaySFX(player.curAbility.SFX_Name);
        }
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Enemy>().photonView.RPC("TakeDamage", RpcTarget.All,player.curAbility.attackPower); // ������ ó��
                //hit.gameObject.SetActive(false);  //�ӽ÷�
            }
            else if(hit.GetComponent<StarBlock>() != null)
            {
                hit.GetComponent<StarBlock>().pv.RPC("Delete", RpcTarget.All); //��� ����
            }
            else if (hit.GetComponent<BigStarBlock>() != null)
            {
                hit.GetComponent<BigStarBlock>().pv.RPC("Delete", RpcTarget.All); //��� ����
            }
        }
        player.curAbility.SFX_Name = ""; //�����Ƽ�� SFX �̸� �ʱ�ȭ
    }
    public void DownAttackTrigger()
    {
        if (player.curAbility == null || !player.pView.IsMine) return; //�����Ƽ�� ������ ����        

        if (AudioManager.Instance != null && player.curAbility.SFX_Name != "")
        {
            //AudioManager.Instance.PlaySFX(player.curAbility.SFX_Name);
            AudioManager.Instance.RPC_PlaySFX(player.curAbility.SFX_Name);
        }
        StartCoroutine(CheckOverlapForSeconds(()=> new Vector2(player.transform.position.x, player.transform.position.y - 1f), player.curAbility.attackCheckRadius, 0.3f)); //���� ���� üũ
        player.curAbility.SFX_Name = ""; //�����Ƽ�� SFX �̸� �ʱ�ȭ
    }
    public void DashAttackTrigger()
    {
        if (player.curAbility == null || !player.pView.IsMine) return; //�����Ƽ�� ������ ����
        if (AudioManager.Instance != null && player.curAbility.SFX_Name != "")
        {
            //AudioManager.Instance.PlaySFX(player.curAbility.SFX_Name);
            AudioManager.Instance.RPC_PlaySFX(player.curAbility.SFX_Name);
        }
        StartCoroutine(CheckOverlapForSeconds(()=>player.attackCheck.position, player.curAbility.attackCheckRadius, 0.3f)); //���� ���� üũ
        player.curAbility.SFX_Name = ""; //�����Ƽ�� SFX �̸� �ʱ�ȭ
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
            rangeAttack = Resources.Load<GameObject>(rangeAttackName); //���Ÿ� ������ ������
        }

        //player.EffectAdd(player.LastMove, rangeAttack, player.AirJumpOutEffectPos); //�÷��̾� ���¼� ���� �߻�
        player.pView.RPC("EffectForCutter", RpcTarget.All, player.LastMove, rangeAttack.name, player.AirJumpOutEffectPos.position); //�÷��̾� ���¼� ���� �߻�
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
                int id = col.GetInstanceID();  // collider ���� ID
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
