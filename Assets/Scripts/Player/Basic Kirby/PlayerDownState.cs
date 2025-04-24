using UnityEngine;

public class PlayerDownState : PlayerState
{
    public PlayerDownState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.lineVelocity(rb.linearVelocityX, 0);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (player.KirbyFormNum == 1) //���� �Կ� ��� �ִ��� �϶� �Ʒ�Ű ���� �ȹٲ�� �ٲ�
        {
            //�Դ� �ִϸ��̼� Down�� �÷��̾� ���������� �ٲٴ� �̺�Ʈ ����
            return;
        }

        if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
            stateMachine.ChangeState(player.idleState);

        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space))
            stateMachine.ChangeState(player.slidingState);

    }
}
