using UnityEngine;

public class Monster_Waddle : Enemy
{

    
    #region States
    public Waddle_IdleState idleState { get; private set; }
    public Waddle_MoveState moveState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState = new Waddle_IdleState(this, stateMachine, "Idle", this);
        moveState = new Waddle_MoveState(this, stateMachine, "Move", this);
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
