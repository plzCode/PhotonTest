using Photon.Pun;
using UnityEngine;

public class PlayerAirJumpingState : PlayerState
{

    public PlayerAirJumpingState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKey(KeyCode.Space))
            stateMachine.ChangeState(player.airJumpUpState);
                
        if (player.IsGroundCheck())
            stateMachine.ChangeState(player.idleState);

        if (xInput != 0)
        {
            //player.lineVelocity(xInput * player.MoveSpeed, rb.linearVelocityY);
            pView.RPC("lineVelocity", RpcTarget.All, xInput * player.MoveSpeed, rb.linearVelocityY);

        }

        if (IsPointerOverItemElement()) return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            stateMachine.ChangeState(player.airJumpOutState);
        }


    }
}
