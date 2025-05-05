using UnityEngine;

public class Sword_Kirby_Attack2_State : PlayerState
{
    private Sword_Kirby_Attack2_End_State attack2_EndState;

    private float attackTime;
    private bool Attack;

    public Sword_Kirby_Attack2_State(Player _player, PlayerStateMachine _stateMachine, string _animBoolName, Sword_Kirby_Attack2_End_State _attack2_EndState)
        : base(_player, _stateMachine, _animBoolName)
    {
        attack2_EndState = _attack2_EndState;
    }

    public override void Enter()
    {
        base.Enter();
        if(pView.IsMine)
        {
            Attack = true;
            attackTime = 3f; // 공격 시간 설정
            player.lineVelocity(0f, rb.linearVelocityY);
            player.curAbility.attackPower = 2f; // 공격력 설정
        }
    }

    public override void Exit()
    {
        base.Exit();
        if (pView.IsMine)
        {
            player.curAbility.attackPower = 10f; // 공격력 설정
        }
    }

    public override void Update()
    {
        base.Update();
        if (!pView.IsMine)
            return;

        attackTime -= Time.deltaTime;
        Debug.Log(attackTime);
        if (attackTime <= 0)
        {
            Attack = false;
        }

        if (Input.GetKeyUp(KeyCode.Mouse0) && attackTime < 2.5f || Attack == false)
        {
            player.stateMachine.ChangeState(attack2_EndState);
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0) && attackTime > 2.5f)
        {
            player.stateMachine.ChangeState(player.idleState);
        }
    }
}
