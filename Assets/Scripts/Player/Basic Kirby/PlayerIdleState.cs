using UnityEngine;
using Photon.Pun;

public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
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

        if (player.dashTime > 0.3f) //�뽬 �ð��� 0.3�� ������ �����̴� Ű ������ �׳� MOVE�� �ٲ�
        {
            player.dash = false;
            player.dashTime = 0;
        }

        if (xInput != 0 && player.dashTime > 0.1f) // 0.1 ~ 0.3�ʾȿ� ������ �� ������ �뽬�� ��ȯ
        {
            stateMachine.ChangeState(player.dashState);
            if (xInput < 0)
            {
                player.turn = true; //���̸� ���Ҷ� �̹��� �¿���� ���� + ������ xInput�� ����
                return;
            }
            else
                player.turn = false; //�����̸� ���Ҷ� �̹��� �¿���� �� + ������ xInput�� ����
            return;
        }

        if (xInput != 0 && !player.isBusy)
        {
            player.dash = true;
            stateMachine.ChangeState(player.moveState);
        }
    }
}
