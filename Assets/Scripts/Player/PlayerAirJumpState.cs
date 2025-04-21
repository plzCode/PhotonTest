using Photon.Realtime;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerAirJumpState : PlayerState
{
    public float AirOutCoolTime;
    public bool AirOut;

    public PlayerAirJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        AirOut = true;
        player.lineVelocity(rb.linearVelocityX, player.MinJumpPower);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (!pView.IsMine) return;
        if (AirOut)
        {
            AirOutCoolTime += Time.deltaTime;
        }

        if (rb.linearVelocityY < -0.1)
            stateMachine.ChangeState(player.airJumpingState);

        if (Input.GetKeyDown(KeyCode.Space))
            stateMachine.ChangeState(player.airJumpUpState);

        if (Input.GetKeyDown(KeyCode.Mouse0) && AirOut && AirOutCoolTime > 0.3)
        {
            stateMachine.ChangeState(player.airJumpOutState);
            AirOut = false;
            AirOutCoolTime = 0;
        }

        if (xInput != 0)
            player.lineVelocity(xInput * player.MoveSpeed, rb.linearVelocityY);
    }
}
