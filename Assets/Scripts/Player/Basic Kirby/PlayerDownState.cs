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
        if (player.KirbyFormNum == 1) //몹을 입에 담고 있는중 일때 아래키 때도 안바뀌게 바꿈
        {
            //먹는 애니메이션 Down에 플레이어 변신폼으로 바꾸는 이벤트 넣음
            return;
        }

        if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
            stateMachine.ChangeState(player.idleState);

        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space))
            stateMachine.ChangeState(player.slidingState);

    }
}
