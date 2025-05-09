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
        pView.RPC("lineVelocity", RpcTarget.All, (player.LastMove * player.MoveSpeed * 2f), 0f); //수평 이동
        player.Flip();
        //공격받을시 흡입 이펙트가 거꾸로 나가는 버그 수정
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
