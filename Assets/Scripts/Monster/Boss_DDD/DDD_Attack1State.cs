using Photon.Pun;
using UnityEngine;

public class DDD_Attack1State : BossState
{
    public bool isMoveing;
    public DDD_Attack1State(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        isMoveing = false;
        boss.isJump = false;
    }

    public override void Update()
    {
        base.Update();

        if(isMoveing == true)
        {
            boss.SetVelocity(7f * boss.facingDir, 0f);
        }


        if (!PhotonNetwork.IsMasterClient)
            return;



        if (boss.isJump)
        {

            boss.photonView.RPC("ChangeState", RpcTarget.All, "Idle");
        }
    }
}
