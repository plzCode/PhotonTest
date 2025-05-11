using Photon.Pun;
using UnityEngine;

public class Bonkers_DieState : BossState
{
    

    public Bonkers_DieState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        boss.anim.SetFloat("yVelocity", 10f);
        if(PhotonNetwork.IsMasterClient)
            boss.photonView.RPC("Die", RpcTarget.All);

    }

    public override void Update()
    {
        
    }

    public override void Exit()
    {
        base.Exit();
    }

    
    
}
