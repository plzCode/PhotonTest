using Photon.Pun;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public float MinJumpPower = 2f;
    public float MaxJumpPower = -1f;

    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        MaxJumpPower = -1f;
        pView.RPC("lineVelocity", RpcTarget.All, rb.linearVelocityX, MinJumpPower + player.JumpPower);
    }

    public override void Exit()
    {
        base.Exit();
        MinJumpPower = 2f;
    }

    public override void Update()
    {
        base.Update();
        if (!pView.IsMine) return;

        if (Input.GetKey(KeyCode.Space) && player.JumpPower >= MaxJumpPower) //�� ������ �ִ��������� ����
        {
            MinJumpPower += 0.05f;
            //player.lineVelocity(rb.linearVelocityX, MinJumpPower + player.JumpPower);
            pView.RPC("lineVelocity", RpcTarget.All, rb.linearVelocityX, MinJumpPower + player.JumpPower); //������ ����
            MaxJumpPower += 0.1f;
        }

        if (rb.linearVelocityY < 0)
        {
            stateMachine.ChangeState(player.airState); ;
        }

        if (xInput != 0)
            //player.lineVelocity(xInput * player.MoveSpeed, rb.linearVelocityY);
            pView.RPC("lineVelocity", RpcTarget.All, xInput * player.MoveSpeed, rb.linearVelocityY); //���� �̵�



        if (player.KirbyFormNum == 1) //���� �Կ� ��� �ִ��� �϶� ���������� �� ���� ����
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
            stateMachine.ChangeState(player.eating12State);


        if (Input.GetKeyDown(KeyCode.Space)) //�����̽��� ������ ���������� ��
        {
            stateMachine.ChangeState(player.airJumpState);

        }
    }
}
