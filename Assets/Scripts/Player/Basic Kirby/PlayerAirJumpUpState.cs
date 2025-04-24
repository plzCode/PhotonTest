using Photon.Pun;
using UnityEngine;

public class PlayerAirJumpUpState : PlayerState
{

    public PlayerAirJumpUpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        //player.lineVelocity(rb.linearVelocityX, player.JumpPower * 1.5f);
        pView.RPC("lineVelocity", RpcTarget.All, rb.linearVelocityX, player.JumpPower * 1.5f);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (rb.linearVelocityY < -0.1f)
            stateMachine.ChangeState(player.airJumpingState);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            stateMachine.ChangeState(player.airJumpOutState);
        }

        if (xInput != 0)
            //player.lineVelocity(xInput * player.MoveSpeed, rb.linearVelocityY);
            pView.RPC("lineVelocity", RpcTarget.All, xInput * player.MoveSpeed, rb.linearVelocityY);
    }
}
