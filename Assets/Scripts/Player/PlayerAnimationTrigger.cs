using UnityEngine;
using Photon.Pun;

public class PlayerAnimatorController : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();

    public void IFGround_Idle_State()
    {
        if (player.IsGroundCheck())
        {
            player.stateMachine.ChangeState(player.idleState);
        }

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
            player.KirbyFrom(); //����
            player.stateMachine.ChangeState(player.changeFormState);
        }
        else
        {
            player.curAbility.OnAbilityDestroyed(player); //0�̸� �⺻ Ŀ��� ��ȯ
            Destroy(player.curAbility);
            player.stateMachine.ChangeState(player.idleState);
        }


    }
}
