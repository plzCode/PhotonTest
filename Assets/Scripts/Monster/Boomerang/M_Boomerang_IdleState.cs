using Photon.Pun;
using UnityEngine;

public class M_Boomerang_IdleState : EnemyState
{

    private Monster_Boomerang enemy;
    private Transform player;
    private float idleTimer;
    public M_Boomerang_IdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Monster_Boomerang _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.SetZeroVelocity();
        idleTimer = enemy.idleTime;
    }

    public override void Update()
    {
        base.Update();
        if (!PhotonNetwork.IsMasterClient) return;
        player = GameManager.Instance.GetClosestPlayer(enemy.transform.position).GetComponent<Transform>();

        idleTimer -= Time.deltaTime;

        
        if (idleTimer < 0 && Vector3.Distance(enemy.transform.position, player.transform.position) <= enemy.playerCheckDistance)
        {
            enemy.photonView.RPC("ChangeState", RpcTarget.All, "Move");
        }
    }

    public override void Exit()
    {
        base.Exit();
    }


}
