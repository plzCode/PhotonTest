using UnityEngine;

public class Sword_Kirby_Attack3_State : PlayerState
{
    private Sword_Kirby_Attack3_1State attack3_1State;
    private float Jump = 15f;

    public Sword_Kirby_Attack3_State(Player _player, PlayerStateMachine _stateMachine, string _animBoolName, Sword_Kirby_Attack3_1State _attack3_1State)
        : base(_player, _stateMachine, _animBoolName)
    {
        attack3_1State = _attack3_1State;
    }

    public override void Enter()
    {
        base.Enter();
        if (!pView.IsMine)
            return;

        player.lineVelocity(player.LastMove, Jump);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (!pView.IsMine)
            return;

        if (rb.linearVelocityY < 10f)
        {
            player.stateMachine.ChangeState(attack3_1State);
        }
    }
}
