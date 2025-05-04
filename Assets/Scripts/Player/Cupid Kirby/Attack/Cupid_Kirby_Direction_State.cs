using UnityEngine;

public class Cupid_Kirby_Direction_State : PlayerState
{
    private Cupid_Kirby_Attack_End_State cupidKirbyAttackEndState;

    public Cupid_Kirby_Direction_State(Player _player, PlayerStateMachine _stateMachine, string _animBoolName,
    Cupid_Kirby_Attack_End_State _attackEndState) : base(_player, _stateMachine, _animBoolName)
    {
        cupidKirbyAttackEndState = _attackEndState;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            player.stateMachine.ChangeState(cupidKirbyAttackEndState);
        }
    }
}
