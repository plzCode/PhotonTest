using Photon.Pun;
using UnityEngine;

public class Wheel_MoveState : EnemyState
{
    private Enemy enemy;

    public Wheel_MoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
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

        if (!PhotonNetwork.IsMasterClient)
        { return; }

        if (stateTimer > 0f)
        {
            enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, rb.linearVelocity.y);
        }
        else if (stateTimer < 0f)
        {
            enemy.SetVelocity(enemy.moveSpeed + 5f * enemy.facingDir, rb.linearVelocity.y);
        }

        else if (enemy.IsWallDetected())
        {
            enemy.photonView.RPC("ChangeState", RpcTarget.All, "Turn");
        }

    }
}
