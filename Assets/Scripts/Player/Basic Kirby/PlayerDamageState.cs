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
        pView.RPC("lineVelocity", RpcTarget.All, (player.EnemyAttackLastPos * player.MoveSpeed * 2f, 0)); //수평 이동
        player.Flip();
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
