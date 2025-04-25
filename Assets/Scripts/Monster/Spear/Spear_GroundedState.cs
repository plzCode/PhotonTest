using UnityEngine;

public class Spear_GroundedState : EnemyState
{
    protected Monster_Spear enemy;



    public Spear_GroundedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Monster_Spear _enemy) : base(_enemy, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
         
        base.Enter();

        
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

    }

    protected Transform GetClosestPlayer()
    {
        Transform closestPlayer = null;
        float shortestDistance = Mathf.Infinity;

        foreach (Transform p in enemy.player)
        {
            if (p == null) continue;

            float distance = Vector3.Distance(enemy.transform.position, p.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestPlayer = p;
            }
        }

        return closestPlayer;
    }

}
