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

        if (player.dashTime > 0.3f) //대쉬 시간이 0.3초 지나면 움직이는 키 눌러도 그냥 MOVE로 바꿈
        {
            player.dash = false;
            player.dashTime = 0;
        }

        if (xInput != 0 && player.dashTime > 0.1f) // 0.1 ~ 0.3초안에 움직임 값 누르면 대쉬로 전환
        {
            stateMachine.ChangeState(player.dashState);
            if (xInput < 0)
            {
                player.turn = true; //참이면 턴할때 이미지 좌우반전 안함 + 마지막 xInput값 저장
                return;
            }
            else
                player.turn = false; //거짓이면 턴할때 이미지 좌우반전 함 + 마지막 xInput값 저장
            return;
        }

        if (xInput != 0 && !player.isBusy)
        {
            player.dash = true;
            stateMachine.ChangeState(player.moveState);
        }
    }
}
