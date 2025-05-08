using Photon.Pun;
using UnityEngine;

public class DDD_AttackState : BossState
{
    private float AttackEndTime;

    public DDD_AttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        AttackEndTime = 0f;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        AttackEndTime += Time.deltaTime;

        if (!PhotonNetwork.IsMasterClient)
            return;

        if (AttackEndTime >= 1f && boss.IsGroundDetected())
        {
            boss.photonView.RPC("ChangeState", RpcTarget.All, "Idle");
        }
    }
}
