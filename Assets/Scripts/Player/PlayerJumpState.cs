using Photon.Pun;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public float MinJumpPower = 2;
    public float MaxJumpPower = -1;

    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        MaxJumpPower = -1;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (!pView.IsMine) return;

        if (Input.GetKeyDown(KeyCode.Space)) //스페이스바 누르면 최소점프
        {
            stateMachine.ChangeState(player.airJumpState);
        }
        else if (Input.GetKey(KeyCode.Space) && player.JumpPower >= MaxJumpPower) //꾹 누르면 최대점프까지 점프
        {
            //player.lineVelocity(rb.linearVelocityX, MinJumpPower + player.JumpPower);
            pView.RPC("lineVelocity", RpcTarget.All, rb.linearVelocityX, MinJumpPower + player.JumpPower); //점프력 증가
            MaxJumpPower += 0.1f;
        }

        if (rb.linearVelocityY < 0)
            stateMachine.ChangeState(player.airState);;

        if (xInput != 0)
            //player.lineVelocity(xInput * player.MoveSpeed, rb.linearVelocityY);
            pView.RPC("lineVelocity", RpcTarget.All, xInput * player.MoveSpeed, rb.linearVelocityY); //수평 이동
    }
}
