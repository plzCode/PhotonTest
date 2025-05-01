using Photon.Pun;
using UnityEngine;

public class M_Sword_MoveState : M_Sword_GroundedState
{
    public M_Sword_MoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        //player = GameObject.FindGameObjectWithTag("Player")?.transform;
        stateTimer = enemy.idleTime;
        // 시간 동기화
        if (Photon.Pun.PhotonNetwork.IsMasterClient)
        {
            stateTimer = enemy.idleTime;
            enemy.photonView.RPC("SyncStateTimer", Photon.Pun.RpcTarget.Others, (float)stateTimer);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();        

        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, rb.linearVelocity.y);

        if (enemy.IsWallDetected()||!enemy.IsGroundDetected())
        {
            enemy.Flip();            
        }

        if (enemy.IsPlayerDetected()&&PhotonNetwork.IsMasterClient)
        {

            //stateTimer = enemy.battleTime;

            if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {

                if (stateTimer<0) // 최소 이동 시간이 지났는가?
                {
                    // sword 몹과 boomerang을 같이 쓰는데 다른점은 combo정도
                    if(enemy.HasParameter("Combo",enemy.anim))
                    {
                        Debug.Log("공격 요청");
                        int randomCombo = Random.Range(1, 3);
                        enemy.photonView.RPC("RequestAnimIntegerChange", RpcTarget.All, "Combo", randomCombo);
                        enemy.photonView.RPC("ChangeState", RpcTarget.All,"Attack");
                    }
                    else
                    {
                        enemy.SetZeroVelocity();
                        enemy.photonView.RPC("ChangeState", RpcTarget.All,"Attack");
                    }
                    
                }

            }
        }
        else
        {
            /*
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 7)
            {
                stateMachine.ChangeState(enemy.moveState);
            }
            */
        }
    }

    /*

    private bool CanAttack()
    {
        if (Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            enemy.lastTimeAttacked = Time.time;
            return true;
        }

        return false;
    }
    */
}
