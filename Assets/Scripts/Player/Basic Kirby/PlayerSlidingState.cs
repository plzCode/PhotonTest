using System;
using UnityEngine;

public class PlayerSlidingState : PlayerState
{
    public float SlidingTime;
    public bool Sliding;

    public PlayerSlidingState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Sliding = true;
        AudioManager.Instance.RPC_PlaySFX("SLIDING");
    }

    public override void Exit()
    {
        base.Exit();
        player.lineVelocity(0f, -1f);
        Sliding = false;
    }

    public override void Update()
    {
        base.Update();
        if (!pView.IsMine) return;

        if (Sliding)
        {
            SlidingTime += Time.deltaTime;
        }

        player.lineVelocity(player.LastMove * player.MoveSpeed * 5, rb.linearVelocityY);

        if (SlidingTime > 0.2f) //0.2�� ������ idle�� ��ȯ �̰� ���� �ڵ带 �����ϰ� �� �� �־���!
        {
            SlidingTime = 0;
            stateMachine.ChangeState(player.idleState);
        }
    }
}
