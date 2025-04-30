using Photon.Pun;
using UnityEngine;

public class Waddle_MoveState : EnemyState
{
    private Enemy enemy;
    public Waddle_MoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemyBase;
    }
    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(!PhotonNetwork.IsMasterClient)
        { return; }
        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, rb.linearVelocity.y);

        if (enemy.IsWallDetected())
        {
            enemy.Flip();
            enemy.photonView.RPC("ChangeState", RpcTarget.All, "Idle");
        }


    }
}
