using UnityEngine;

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

    public void Air_State()
    {
        player.stateMachine.ChangeState(player.airState);
    }
}
