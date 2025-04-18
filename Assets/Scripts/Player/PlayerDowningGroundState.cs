using UnityEngine;

public class PlayerDowningGroundState : PlayerState
{
    public PlayerDowningGroundState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public float move;
    public bool ground;

    public override void Enter()
    {
        base.Enter();

        if(player.flipbool)
        {
            move = 1;
        }
        else
        {
            move = -1;
        }

            player.lineVelocity(move * player.MoveSpeed, player.MinJumpPower);
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
