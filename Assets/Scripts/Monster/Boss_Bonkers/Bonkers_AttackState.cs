using Photon.Pun;
using UnityEngine;

public class Bonkers_AttackState : BossState
{
    public bool isBackWalking;

    public Bonkers_AttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        base.Update();

        

        if(isBackWalking)
        {
            boss.rb.linearVelocity = new Vector2(3 * -boss.facingDir, 0);
        }

        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

        if (triggerCalled)
        {
            boss.photonView.RPC("ChangeState", RpcTarget.All, "Idle");
        }
    }

    public override void Exit()
    {
        base.Exit();
        isBackWalking = false;
    }

    
}
