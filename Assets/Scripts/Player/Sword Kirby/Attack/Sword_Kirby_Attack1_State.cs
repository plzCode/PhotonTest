using UnityEngine;

public class Sword_Kirby_Attack1_State : PlayerState
{
    private Sword_Kirby_Attack2_State attack2State;

    public Sword_Kirby_Attack1_State(Player _player, PlayerStateMachine _stateMachine, string _animBoolName, Sword_Kirby_Attack2_State _attack2State)
        : base(_player, _stateMachine, _animBoolName)
    {
        attack2State = _attack2State;
    }

    public override void Enter()
    {
        base.Enter();
        if (pView.IsMine)
        {
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

        if (Input.GetKey(KeyCode.Mouse0))
        {
            player.stateMachine.ChangeState(attack2State);
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            player.stateMachine.ChangeState(player.idleState);
        }
    }
}
