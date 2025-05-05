using Photon.Pun;
using UnityEngine;

public class Cutter_Kirby_Attack_State : PlayerState
{
    private Cutter_Kirby_Attack_End_State cutterKirbyAttackEndState;

    public Cutter_Kirby_Attack_State(Player _player, PlayerStateMachine _stateMachine, string _animBoolName,
    Cutter_Kirby_Attack_End_State _attackEndState) : base(_player, _stateMachine, _animBoolName)
    {
        cutterKirbyAttackEndState = _attackEndState;
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
            if (pView.IsMine)
            {
                player.stateMachine.ChangeState(cutterKirbyAttackEndState);
            }
        }
    }
}
