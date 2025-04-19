using UnityEngine;

public class Monster_03 : Enemy
{

    
    #region States
    public M03_IdleState idleState { get; private set; }
    public M03_MoveState moveState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState = new M03_IdleState(this, stateMachine, "Idle", this);
        moveState = new M03_MoveState(this, stateMachine, "Move", this);
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
}
