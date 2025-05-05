using UnityEngine;

public class Sword_Kirby_Attack2_End_State : PlayerState
{
    public Sword_Kirby_Attack2_End_State(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if (pView.IsMine)
        {
            player.curAbility.attackCheckRadius = 0.8f; // 공격력 설정
            player.lineVelocity(0f, rb.linearVelocityY);
        }

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
