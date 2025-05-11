using Photon.Pun;
using UnityEngine;

public class DDD_DieState : BossState
{
    public DDD_DieState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        boss.anim.SetFloat("yVelocity", 10f);
        if (PhotonNetwork.IsMasterClient)
            boss.photonView.RPC("Die", RpcTarget.All);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}
