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
            findPlayerCounter = 3; //��Ž�� �ϴ� ������ ������ �Ұ��ΰ�? �����ص��ɵ� 1~3���� -> ���� 1~3�ϰ��� ��Ž����. ���Ͻ��۸��� ī��Ʈ���� �ʿ�
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
