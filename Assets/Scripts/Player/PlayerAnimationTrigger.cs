using UnityEngine;
using Photon.Pun;
<<<<<<< HEAD
using Unity.VisualScripting;
=======
using System.Collections.Generic;
using System.Collections;
using System;
using System.Linq;
>>>>>>> aeb81e59c3b1de5535fc169bf919662e2ea0e5aa

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

        player.KirbyFormNum = player.EatKirbyFormNum; //����Ǿ��ִ� �� �ѹ��� �����ϴ� �ѹ��� �ѱ��
        player.GetComponent<PhotonView>().RPC("SyncFormNum", RpcTarget.AllBuffered);

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
            //AudioManager.Instance.PlaySFX(player.curAbility.SFX_Name);
            AudioManager.Instance.GetComponent<PhotonView>().RPC("RPC_PlaySFX", RpcTarget.All, player.curAbility.SFX_Name);
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

        Debug.Log("DownAttack !!!!!!!!!!!!!!!!!!!!");
        if (AudioManager.Instance != null && player.curAbility.SFX_Name != "")
        {
            //AudioManager.Instance.PlaySFX(player.curAbility.SFX_Name);
            AudioManager.Instance.GetComponent<PhotonView>().RPC("RPC_PlaySFX", RpcTarget.All, player.curAbility.SFX_Name);
        }
        StartCoroutine(CheckOverlapForSeconds(()=> new Vector2(player.transform.position.x, player.transform.position.y - 1f), player.curAbility.attackCheckRadius, 0.3f)); //���� ���� üũ
        player.curAbility.SFX_Name = ""; //�����Ƽ�� SFX �̸� �ʱ�ȭ
    }
    public void DashAttackTrigger()
    {
        if (player.curAbility == null || !player.pView.IsMine) return; //�����Ƽ�� ������ ����
        Debug.Log("DashAttack !!!!!!!!!!!!!!!!!!!!");
        if (AudioManager.Instance != null && player.curAbility.SFX_Name != "")
        {
            //AudioManager.Instance.PlaySFX(player.curAbility.SFX_Name);
            AudioManager.Instance.GetComponent<PhotonView>().RPC("RPC_PlaySFX", RpcTarget.All, player.curAbility.SFX_Name);
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


    private GameObject Attack;

    private void RangedAttack(GameObject rangeAttack, string rangeAttackName)
    {
        if (rangeAttack == null)
        {
            rangeAttack = Resources.Load<GameObject>(rangeAttackName); //���Ÿ� ������ ������
        }

        player.EffectAdd(player.LastMove, rangeAttack, player.AirJumpOutEffectPos); //�÷��̾� ���¼� ���� �߻�
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
}
