using UnityEngine;

public class Monster_Sword : Enemy
{
    [Header("공격 대쉬")]
    [SerializeField] private float dashSpeed;


    #region States
    public M_Sword_MoveState moveState { get; private set; }
    public M_Sword_AttackState attackState { get; private set; }

    #endregion
    protected override void Awake()
    {
        base.Awake();

        moveState = new M_Sword_MoveState(this, stateMachine, "Move", this);
        attackState = new M_Sword_AttackState(this, stateMachine, "Attack", this);
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(moveState);
    }

    protected override void Update()
    {
        base.Update();
    }

    public void Sword_AttackForward()
    {
        SetVelocity(dashSpeed * facingDir, rb.linearVelocity.y);
    }
}
