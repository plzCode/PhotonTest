using Photon.Pun;
using UnityEngine;

public class M_Mole_IdleState : EnemyState
{
    private Enemy enemy;
    private Transform player;


    public M_Mole_IdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemyBase;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.canAttacking = false;
        if (PhotonNetwork.IsMasterClient)
            enemy.SetZeroVelocity();
        

    }

    public override void Update()
    {
        base.Update();

        player = GameManager.Instance.GetClosestPlayer(enemy.transform.position).GetComponent<Transform>();

        // 조건: 마스터 클라이언트 && 플레이어 존재 && 거리 5 이하
        if (PhotonNetwork.IsMasterClient && player != null && Vector2.Distance(player.position, enemy.transform.position) <= 8f)
        {
            enemy.photonView.RPC("ChangeState", RpcTarget.All, "Battle");
        }

    }

    public override void Exit()
    {
        base.Exit();
    }

    void OnBecameVisible()
    {
        //stateMachine.ChangeState(enemy.battleState);
    }

}
