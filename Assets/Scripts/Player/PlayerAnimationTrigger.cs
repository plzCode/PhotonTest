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
}
