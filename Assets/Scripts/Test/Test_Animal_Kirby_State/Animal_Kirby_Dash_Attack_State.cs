using UnityEngine;

public class Animal_Kirby_Dash_Attack_State : PlayerState
{
    public Animal_Kirby_Dash_Attack_State(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    private float duration = 0.5f;
    private float elapsedTime;

    public override void Enter()
    {
        base.Enter();
        elapsedTime = 0f;
        if(player.flipbool)
        {
            player.lineVelocity(10f, 0f);
        }
        else
        {
            player.lineVelocity(-10f, 0f);
        }

        player.rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        player.rb.freezeRotation = true;

    }

    public override void Exit()
    {
        base.Exit(); 
        player.rb.constraints = RigidbodyConstraints2D.None;
        player.rb.freezeRotation = true;
    }

    public override void Update()
    {
        base.Update();
        if (triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }

        elapsedTime += Time.deltaTime;

        if (elapsedTime >= duration)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

}
