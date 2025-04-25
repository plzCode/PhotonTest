using UnityEngine;

public class M04_DieState : EnemyState
{
    private Monster_Mon04 enemy;

    public M04_DieState(Enemy _enemyBase, EnemyStateMachine _sm, string _animTrigger)
        : base(_enemyBase, _sm, _animTrigger)
    {
        enemy = (Monster_Mon04)_enemyBase;
    }

    public override void Enter()
    {
        triggerCalled = false;
        enemy.anim.SetTrigger(animBoolName);   // "Die"
        enemy.SetZeroVelocity();
        var col = enemy.GetComponent<Collider2D>();
        if (col != null) col.enabled = false;
    }

    public override void Update()
    {
        if (triggerCalled)
            GameObject.Destroy(enemy.gameObject);
    }

    public override void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
