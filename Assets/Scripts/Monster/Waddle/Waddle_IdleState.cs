using Photon.Pun;
using UnityEngine;

public class Waddle_IdleState : EnemyState
{
    private Enemy enemy;
    public Waddle_IdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
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

        
        if (stateTimer < 0 && PhotonNetwork.IsMasterClient)
        {
            enemy.photonView.RPC("ChangeState", RpcTarget.All, "Move");
        }

    }
}
