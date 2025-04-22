using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    private Player player => GetComponentInParent<Player>();

    public void IdleChange()
    {
        if (player.IsGroundCheck())
        {
            player.stateMachine.ChangeState(player.idleState);
        }

        //���ϸ����Ϳ��� idle�� ������
    }
}
