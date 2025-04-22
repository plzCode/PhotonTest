using Photon.Pun;
using UnityEngine;

public class PlayerAirJumpingState : PlayerState
{
    public float AirOutCoolTime;
    public bool AirOut;

    public PlayerAirJumpingState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        AirOut = true;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (AirOut)
        {
            AirOutCoolTime += Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.Space))
            stateMachine.ChangeState(player.airJumpUpState);

        if (Input.GetKeyDown(KeyCode.Mouse0) && AirOut && AirOutCoolTime > 0.3f)
        {
            stateMachine.ChangeState(player.airJumpOutState);
            AirOut = false;
            AirOutCoolTime = 0;
        }

        if (player.IsGroundCheck())
            stateMachine.ChangeState(player.idleState);

        if (xInput != 0)
        {
            //player.lineVelocity(xInput * player.MoveSpeed, rb.linearVelocityY);
            pView.RPC("lineVelocity", RpcTarget.All, xInput * player.MoveSpeed, rb.linearVelocityY);

        }

        
    }
}
