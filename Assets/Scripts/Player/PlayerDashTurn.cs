using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerDashTurn : PlayerState
{

    public PlayerDashTurn(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.dash = true;
        player.dashTime = 0;
        player.Flip();
        player.ZerolineVelocity(0, 0);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (xInput != 0 && player.dashTime > 0.2)
        {
            if (xInput < 0)
            {
                player.dashTime = 0;
                player.turn = true;
                player.Flip();
                stateMachine.ChangeState(player.dashState);
            }
            else if (xInput > 0)
            {
                player.dashTime = 0;
                player.turn = false;
                player.Flip();
                stateMachine.ChangeState(player.dashState);
            }
        }

        if (xInput == 0 && player.dashTime > 0.2)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }





}
