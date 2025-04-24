using UnityEngine;
using Photon.Pun;

public class PlayerDamageState : PlayerState
{
    public PlayerDamageState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        pView.RPC("lineVelocity", RpcTarget.All, (player.LastMove * player.MoveSpeed * 2f), 0f); //수평 이동
        player.Flip();
    }

    public override void Exit()
    {
        base.Exit();
        if (player.LastMove == 1f)
        {
            player.LastMove = -1f;
        }
        else
            player.LastMove = 1f;
    }

    public override void Update()
    {
        base.Update();
    }
}
