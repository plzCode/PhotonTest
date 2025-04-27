using Photon.Pun;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public float coolTime;

    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        coolTime = 0;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        coolTime += Time.deltaTime;

        if (!player.TimeBool && coolTime > 0.4f)
        {
            player.TimeBool = true;
        }


        if (player.IsGroundCheck())
        {
            player.EffectAdd(1f, player.GroundStarEffect, player.GroundEffectPos);
            stateMachine.ChangeState(player.idleState);
            return;
        }

        if (xInput != 0)
            //player.lineVelocity(xInput * player.MoveSpeed, rb.linearVelocityY);
            pView.RPC("lineVelocity", RpcTarget.All, xInput * player.MoveSpeed, rb.linearVelocityY); //���� �̵�




        if (player.KirbyFormNum == 1) //���� �Կ� ��� �ִ��� �϶� ���������� ������ �߶������� ��� ���ϰ� ����
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && player.KirbyFormNum == 0)
            stateMachine.ChangeState(player.eating12State);


        if (Input.GetKeyDown(KeyCode.Space) && player.TimeBool)
        {
            //player.lineVelocity(rb.linearVelocityX, player.JumpPower);
            pView.RPC("lineVelocity", RpcTarget.All, rb.linearVelocityX, player.JumpPower);
            stateMachine.ChangeState(player.airJumpState);
        }

        if (rb.linearVelocityY < -8f)
            stateMachine.ChangeState(player.downingState);
    }
}
