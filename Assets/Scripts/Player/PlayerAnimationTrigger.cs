using UnityEngine;
using Photon.Pun;

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

    public void Idle_state()
    {
        player.stateMachine.ChangeState(player.idleState);
    }

    public void Air_State()
    {
        player.stateMachine.ChangeState(player.airState);
    }

    public void ChangeForm()
    {
        player.KirbyFormNum = player.EatKirbyFormNum; //����Ǿ��ִ� �� �ѹ��� �����ϴ� �ѹ��� �ѱ��

        if (player.KirbyFormNum > 0) //0�̻��̸� ���� �� Ŀ��� ����
        {
            //player.KirbyFrom(); //����
            player.Call_RPC("KirbyFrom", RpcTarget.All); //����
            player.stateMachine.ChangeState(player.changeFormState);
        }
        else
        {
            player.curAbility.OnAbilityDestroyed(player); //0�̸� �⺻ Ŀ��� ��ȯ
            Destroy(player.curAbility);
            player.stateMachine.ChangeState(player.idleState);
        }
    }

    public void AttackTrigger()
    {
        if(player.curAbility == null) return; //�����Ƽ�� ������ ����
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.curAbility.attackCheckRadius);
        Debug.Log(player.curAbility.attackCheckRadius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                Debug.Log("������" + player.curAbility.attackPower+ "��ŭ �������� ��");
                hit.GetComponent<Enemy>().TakeDamage(player.curAbility.attackPower);
                hit.gameObject.SetActive(false);  //�ӽ÷�
            }
        }
    }
}
