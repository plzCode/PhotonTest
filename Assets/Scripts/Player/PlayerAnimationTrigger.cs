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

        //에니메이터에서 idle로 가게함
    }
}
