using Photon.Pun;
using UnityEngine;

public class Cat_IdleState : EnemyState
{
    private Enemy enemy;
    public Cat_IdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemyBase;
    }
    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.idleTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(!PhotonNetwork.IsMasterClient)
            return;


        if (stateTimer < 0)
        {
            enemy.photonView.RPC("ChangeState", RpcTarget.All, "Move");
        }

        else if (enemy.IsWallDetected())
        {
            enemy.Flip();
            enemy.photonView.RPC("ChangeState", RpcTarget.All, "Idle");
        }

    }
}
