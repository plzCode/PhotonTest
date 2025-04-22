using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerAirJumpState : PlayerState
{
    public float MinJumpPower = 2f;

    public float AirOutCoolTime;
    public bool AirOut;

    public PlayerAirJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        AirOut = true;
        //player.lineVelocity(rb.linearVelocityX, MinJumpPower);
        pView.RPC("lineVelocity", RpcTarget.All, rb.linearVelocityX, MinJumpPower);
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

        if (rb.linearVelocityY < -0.1f)
            stateMachine.ChangeState(player.airJumpingState);

        if (Input.GetKeyDown(KeyCode.Space))
            stateMachine.ChangeState(player.airJumpUpState);

        if (Input.GetKeyDown(KeyCode.Mouse0) && AirOut && AirOutCoolTime > 0.3) //계속 공기 뱉을수 없게 0.3으로 조정
        {
            stateMachine.ChangeState(player.airJumpOutState);
            AirOut = false;
            AirOutCoolTime = 0;
        }

        if (xInput != 0)
            pView.RPC("lineVelocity", RpcTarget.All, xInput * player.MoveSpeed, rb.linearVelocityY);
            //player.lineVelocity(xInput * player.MoveSpeed, rb.linearVelocityY);
    }
}
