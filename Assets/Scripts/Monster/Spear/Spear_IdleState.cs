
using System.Collections;
using UnityEngine;


public class Spear_IdleState : Spear_GroundedState
{
    public Spear_IdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Monster_Spear _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
    {

    }
    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.idleTime;

        // 시간 동기화
        if (Photon.Pun.PhotonNetwork.IsMasterClient)
        {
            enemy.photonView.RPC("SyncStateTimer", Photon.Pun.RpcTarget.Others, stateTimer);
            
        }

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        Debug.Log(stateTimer);
        GameObject closestPlayer = GameManager.Instance.GetClosestPlayer(enemy.transform.position);
        if (closestPlayer == null) return;

        Transform target = closestPlayer.transform;
        float distanceToTarget = Vector3.Distance(enemy.transform.position, target.position);

        if (distanceToTarget <= enemy.throwDistance + 2f)
        {
            if (target.position.x < enemy.transform.position.x)
            {
                enemy.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else if (target.position.x > enemy.transform.position.x)
            {
                enemy.transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            if (stateTimer < 0)
            {
                if (distanceToTarget <= enemy.throwDistance)
                {
                    stateMachine.ChangeState(enemy.throwState);
                }
            }
        }





    }

    
}
