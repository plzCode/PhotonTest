using Photon.Pun;
using UnityEngine;

public class DDD_Attack3State : BossState
{
    public DDD_Attack3State(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
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

        if (boss.isJump)
        {

            boss.photonView.RPC("ChangeState", RpcTarget.All, "Idle");
        }
    }
}
