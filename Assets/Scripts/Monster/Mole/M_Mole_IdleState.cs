using UnityEngine;

public class M_Mole_IdleState : EnemyState
{
    private Monster_Mole enemy;


    public M_Mole_IdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Monster_Mole _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.SetZeroVelocity();
        
    }

    public override void Update()
    {
        base.Update();

        if (enemy.GetComponentInChildren<Renderer>().isVisible)
        {
            stateMachine.ChangeState(enemy.battleState);
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
