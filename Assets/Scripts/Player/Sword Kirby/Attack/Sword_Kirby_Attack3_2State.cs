using UnityEngine;

public class Sword_Kirby_Attack3_2State : PlayerState
{
    private Sword_Kirby_Attack3_End_State attack3_EndState;
    private float DownJump = -15f;

    public Sword_Kirby_Attack3_2State(Player _player, PlayerStateMachine _stateMachine, string _animBoolName, Sword_Kirby_Attack3_End_State _attack3_EndState)
        : base(_player, _stateMachine, _animBoolName)
    {
        attack3_EndState = _attack3_EndState;
    }

    public override void Enter()
    {
        base.Enter();
        if (!pView.IsMine)
            return;

        player.lineVelocity(player.LastMove, DownJump);
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

        if (player.IsGroundCheck())
        {
            player.stateMachine.ChangeState(attack3_EndState);
        }
    }
}
