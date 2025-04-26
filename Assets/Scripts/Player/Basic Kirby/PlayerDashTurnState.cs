using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerDashTurnState : PlayerState
{

    public PlayerDashTurnState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.dash = true;
        player.dashTime = 0f;
        player.Flip();
        player.lineVelocity(0f, rb.linearVelocityY);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (xInput != 0 && player.dashTime > 0.2) //턴 모션 나오자마자 바로 대쉬하면 안되니 0.2초 이상 제한 검
        {
            if (xInput < 0)
            {
                player.dashTime = 0;
                player.turn = true; //참이면 턴할때 이미지 좌우반전 안함 + 마지막 xInput값 저장
                player.Flip();
                stateMachine.ChangeState(player.dashState);
            }
            else if (xInput > 0)
            {
                player.dashTime = 0;
                player.turn = false; //거짓이면 턴할때 이미지 좌우반전 함 + 마지막 xInput값 저장
                player.Flip();
                stateMachine.ChangeState(player.dashState);
            }
        }

        if (xInput == 0 && player.dashTime > 0.2) //0.2초 안에 가만히 있으면 idle로 바꿈
        {
            stateMachine.ChangeState(player.idleState);
        }
    }





}
