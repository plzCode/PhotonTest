using Photon.Realtime;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerAirJumpState : PlayerState
{
    public PlayerAirJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.lineVelocity(rb.linearVelocityX, player.MinJumpPower);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        player.LastInput(xInput);

        if (rb.linearVelocityY < -0.1)
            stateMachine.ChangeState(player.airJumpingState);

        if (Input.GetKeyDown(KeyCode.Space))
            stateMachine.ChangeState(player.airJumpUpState);

        if (Input.GetKeyDown(KeyCode.Z))
            stateMachine.ChangeState(player.airJumpOutState);

        if (xInput != 0)
            player.lineVelocity(xInput * player.MoveSpeed, rb.linearVelocityY);
    }
}
