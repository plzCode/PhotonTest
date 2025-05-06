using UnityEngine;

public class PlayerEating4State : PlayerState
{
    public PlayerEating4State(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.lineVelocity(0f, 0f); //������ 0���� �����
        AudioManager.Instance.RPC_StopSFX();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}
