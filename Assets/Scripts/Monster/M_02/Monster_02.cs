using UnityEngine;

public class Monster_02 : Enemy
{
    [Header("M02 원거리 공격 정보")]
    [SerializeField] public float throwDistance;


    #region States
    public M02_IdleState idleState { get; private set; }
    public M02_ThrowState throwState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState = new M02_IdleState(this, stateMachine, "Idle", this);
        throwState = new M02_ThrowState(this, stateMachine, "Throw", this);
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);

    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, throwDistance);
    }

}
