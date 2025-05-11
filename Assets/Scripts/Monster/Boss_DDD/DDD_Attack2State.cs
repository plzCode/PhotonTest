using Photon.Pun;
using UnityEngine;

public class DDD_Attack2State : BossState
{
    private bool isJumping;

    public DDD_Attack2State(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
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
    }
}
