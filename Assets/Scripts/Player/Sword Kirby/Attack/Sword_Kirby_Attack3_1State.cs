using UnityEngine;

public class Sword_Kirby_Attack3_1State : PlayerState
{
    private Sword_Kirby_Attack3_2State attack3_2State;
    private float CoolTime;

    public Sword_Kirby_Attack3_1State(Player _player, PlayerStateMachine _stateMachine, string _animBoolName, Sword_Kirby_Attack3_2State _attack3_2State)
        : base(_player, _stateMachine, _animBoolName)
    {
        attack3_2State = _attack3_2State;
    }

    public override void Enter()
    {
        base.Enter();
        if (!pView.IsMine)
            return;

        CoolTime = 0.5f;
        player.lineVelocity(player.LastMove, 0f);
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

        CoolTime -= Time.deltaTime;

        if (CoolTime <= 0)
        {
            player.stateMachine.ChangeState(attack3_2State);
        }
    }
}
