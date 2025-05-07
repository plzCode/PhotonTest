using Photon.Pun;
using UnityEngine;

public class Whell_Kirby_Attack_State : PlayerState
{
    private PlayerState nextState;
    public Whell_Kirby_Attack_End attackEnd;
    private bool audio;
    private float audioTime;

    public Whell_Kirby_Attack_State(Player _player, PlayerStateMachine _stateMachine, string _animBoolName, Whell_Kirby_Attack_End _attackEnd)
        : base(_player, _stateMachine, _animBoolName)
    {
        attackEnd = _attackEnd;
    }

    public void SetNextState(PlayerState _nextState)
    {
        nextState = _nextState;
    }

    public override void Enter()
    {
        base.Enter();
        audio = true;
        audioTime = 1.5f;
    }

    public override void Exit()
    {
        base.Exit();
        AudioManager.Instance.RPC_StopSFX();
    }

    public override void Update()
    {
        base.Update();
        if (!pView.IsMine)
            return;

        audioTime += Time.deltaTime;

        if(audioTime > 0.1f)
        {
            audio = true;
        }

        if (audio)
        {
            AudioManager.Instance.RPC_PlaySFX("kirby_WHEEL_RUN");
            audioTime = 0f;
            audio = false;
        }

        player.lineVelocity(player.LastMove * player.MoveSpeed * 3f, rb.linearVelocityY);

        if(Input.GetKeyDown(KeyCode.S))
        {
            player.stateMachine.ChangeState(attackEnd);
        }

        if (Input.GetKeyDown(KeyCode.Space) && player.IsGroundCheck())
        {
            pView.RPC("lineVelocity", RpcTarget.All, rb.linearVelocityX, player.JumpPower + 5f);
        }

        if (player.LastMove == 1f)
        {
            if (xInput == -1f)
            {
                player.stateMachine.ChangeState(nextState);
            }
        }

        if (player.LastMove == -1f)
        {
            if (xInput == 1f)
            {
                player.stateMachine.ChangeState(nextState);
            }
        }
    }
}
