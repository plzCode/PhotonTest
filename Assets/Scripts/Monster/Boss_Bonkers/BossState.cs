using UnityEngine;

public class BossState : EnemyState
{
    public Enemy boss;
    public Transform closestPlayer;
    public bool isJumpTurn;
    public float specificTime;
    public int randomJumpCount;
    public int randAttackCount;

    public int findPlayerCounter=3;
    public bool isActive;

    public BossState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        boss = _enemyBase;
    }

    public override void Enter()
    {
        base.Enter();

        if (findPlayerCounter < 0)
        {
            closestPlayer = GameManager.Instance.GetClosestPlayer(boss.transform.position).GetComponent<Transform>();
            findPlayerCounter = 3; //재탐색 하는 기준을 몇으로 할것인가? 랜덤해도될듯 1~3정도 -> 패턴 1~3하고나서 재탐색함. 패턴시작마다 카운트감소 필요
        }
    }

    public override void Exit()
    {
        base.Exit();

        findPlayerCounter--;
    }

    public override void Update()
    {
        base.Update();

        specificTime -= Time.deltaTime;     


        if(closestPlayer==null)
        {
            closestPlayer = GameManager.Instance.GetClosestPlayer(boss.transform.position).GetComponent<Transform>();
        }

        Debug.Log(closestPlayer);

    }
}
