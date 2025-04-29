using Photon.Pun;
using UnityEngine;

public class M_Sword_AttackState : M_Sword_GroundedState
{
    public M_Sword_AttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Monster_Sword _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
    }


    public override void Update()
    {
        base.Update();

        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        
        if (triggerCalled)
        {
            enemy.photonView.RPC("ChangeState", RpcTarget.All, "Move");
        }

        if (enemy.IsWallDetected() || !enemy.IsGroundDetected())
        {
            enemy.SetZeroVelocity();
        }
    }

    public override void Exit()
    {
        base.Exit();

        //enemy.lastTimeAttacked = Time.time;
    }

    
}
