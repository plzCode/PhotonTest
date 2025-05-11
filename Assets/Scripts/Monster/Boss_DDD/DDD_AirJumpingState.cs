using Photon.Pun;
using UnityEngine;

public class DDD_AirJumpingState : BossState
{
    public DDD_AirJumpingState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        boss.isJump = false;
    }

    public override void Update()
    {
        base.Update();

        if (!PhotonNetwork.IsMasterClient)
            return;

        if (boss.isJump && boss.IsGroundDetected())
        {
            boss.photonView.RPC("ChangeState", RpcTarget.All, "Idle");
        }


        if (Mathf.Abs(closestPlayer.position.x - boss.transform.position.x) <= 1f ||
                (closestPlayer.position.x < boss.transform.position.x && boss.facingDir == 1) ||
                (closestPlayer.position.x > boss.transform.position.x && boss.facingDir == -1))
        {
            boss.photonView.RPC("ChangeState", RpcTarget.All, "Jump");
        }
    }

}
