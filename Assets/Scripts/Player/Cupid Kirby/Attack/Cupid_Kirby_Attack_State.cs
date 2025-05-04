using UnityEngine;

public class Cupid_Kirby_Attack_State : PlayerState
{
    private Cupid_Kirby_Direction_State cupidKirbyDirectionState;

    public Cupid_Kirby_Attack_State(Player _player, PlayerStateMachine _stateMachine, string _animBoolName,
        Cupid_Kirby_Direction_State _attackDirectionState) 
        : base(_player, _stateMachine, _animBoolName)
    {
        cupidKirbyDirectionState = _attackDirectionState;
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

        if (Input.GetKey(KeyCode.Mouse0))
        {
            player.stateMachine.ChangeState(cupidKirbyDirectionState);
        }
    }
}
