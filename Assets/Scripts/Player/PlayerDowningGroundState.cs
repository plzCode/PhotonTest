using UnityEngine;

public class PlayerDowningGroundState : PlayerState
{
    public float Lastmove;
    public int ground;

    public PlayerDowningGroundState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if(player.flipbool)
        {
            Lastmove = 1;
        }
        else
        {
            Lastmove = -1;
        }

            player.lineVelocity(Lastmove * player.MoveSpeed, player.MinJumpPower);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.lineVelocity(xInput * player.MoveSpeed, rb.linearVelocityY);

        if (player.IsGroundCheck())
        {
            ground += 1;

            if (player.IsGroundCheck() && ground >= 15)
            {
                ground = 0;
                stateMachine.ChangeState(player.idleState);
            }
        }
    }
}
