using UnityEngine;

public class Whell_Kirby_Attack_Ready_State : PlayerState
{
    private PlayerState nextState;
    private bool audio;

    public Whell_Kirby_Attack_Ready_State(Player _player, PlayerStateMachine _stateMachine, string _animBoolName, PlayerState _nextState)
        : base(_player, _stateMachine, _animBoolName)
    {
        nextState = _nextState;
    }

    public void SetNextState(PlayerState _nextState)
    {
        nextState = _nextState;
    }


    public override void Enter()
    {
        base.Enter();
        audio = true;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (!pView.IsMine)
            return;

        if (audio)
        {
            AudioManager.Instance.RPC_PlaySFX("kirby_WHEEL_START");
            audio = false;
        }

        if(triggerCalled)
        {
            player.stateMachine.ChangeState(nextState);
        }
    }
}
