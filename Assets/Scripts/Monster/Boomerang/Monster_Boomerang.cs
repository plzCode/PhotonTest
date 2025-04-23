using UnityEngine;

public class Monster_Boomerang : Monster_Sword
{
    [Header("부메랑 몬스터 플레이어 탐지 거리")]
    [SerializeField] public float playerCheckDistance;


    public M_Boomerang_IdleState idleState{ get; private set; }

    protected override void Awake()
    {
        base.Awake();

        idleState = new M_Boomerang_IdleState(this, stateMachine, "Idle", this);
    }

    protected override void Start()
    {
        setStart();

        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerCheckDistance); // 탐지 사거리
        
    }
}
