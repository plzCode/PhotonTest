using UnityEngine;

public class Sword_Kirby_Attack3_End_State : PlayerState
{
    public Sword_Kirby_Attack3_End_State(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if (!pView.IsMine)
            return;

        player.lineVelocity(player.LastMove, rb.linearVelocityY);
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
