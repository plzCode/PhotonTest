using UnityEngine;

public class Monster_Mole : Enemy
{

    #region States
    public M_Mole_IdleState idleState { get; private set; }
    public M_Mole_BattleState battleState { get; private set; }
    #endregion
    protected override void Awake()
    {
        base.Awake();

        idleState = new M_Mole_IdleState(this, stateMachine, "Idle", this);
        battleState = new M_Mole_BattleState(this, stateMachine, "AttackJump", this);
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
