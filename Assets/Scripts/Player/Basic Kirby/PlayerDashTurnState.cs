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
        player.lineVelocity(0f, -1f);
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

        if (xInput != 0f && player.dashTime > 0.2f) //�� ��� �����ڸ��� �ٷ� �뽬�ϸ� �ȵǴ� 0.2�� �̻� ���� ��
        {
            if (xInput < 0f)
            {
                player.dashTime = 0f;
                player.turn = true; //���̸� ���Ҷ� �̹��� �¿���� ���� + ������ xInput�� ����
                player.Flip();
                stateMachine.ChangeState(player.dashState);
            }
            else if (xInput > 0f)
            {
                player.dashTime = 0f;
                player.turn = false; //�����̸� ���Ҷ� �̹��� �¿���� �� + ������ xInput�� ����
                player.Flip();
                stateMachine.ChangeState(player.dashState);
            }
        }

        if (xInput == 0f && player.dashTime > 0.2f) //0.2�� �ȿ� ������ ������ idle�� �ٲ�
        {
            stateMachine.ChangeState(player.idleState);
        }
    }





}
