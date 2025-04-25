using UnityEngine;

public class M04_Attack1State : EnemyState
{
    private Monster_Mon04 enemy;

    public M04_Attack1State(Enemy _enemyBase, EnemyStateMachine _sm, string _animTrigger)
        : base(_enemyBase, _sm, _animTrigger)
    {
        enemy = (Monster_Mon04)_enemyBase;
    }

    public override void Enter()
    {
        triggerCalled = false;
        enemy.anim.SetTrigger(animBoolName);   
        enemy.SetZeroVelocity();
        enemy.lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        if (triggerCalled)
        {
            enemy.lastTimeAttacked = Time.time;
            stateMachine.ChangeState(enemy.walkState);  
        }
    }

    public override void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
