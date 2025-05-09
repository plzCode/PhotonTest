using Photon.Pun;
using UnityEngine;

public class PlayerDieState : PlayerState
{
    public PlayerDieState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter(); 
        pView.RPC("lineVelocity", RpcTarget.All, (player.LastMove * player.MoveSpeed * 2f), 0f); //���� �̵�
        player.Flip();
        //���ݹ����� ���� ����Ʈ�� �Ųٷ� ������ ���� ����
        if (player.LastMove == 1f)
        {
            player.LastMove = -1f;
        }
        else
            player.LastMove = 1f;
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();
    }
}
