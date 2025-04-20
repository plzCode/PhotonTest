using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Windows;
using Input = UnityEngine.Input;

public class PlayerState
{
    protected Player player;
    protected PlayerStateMachine stateMachine;

    protected float xInput;

    protected Rigidbody2D rb;

    private string animBoolName;
    protected PhotonView playerView;

    
    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
        try { playerView = this.player.GetComponent<PhotonView>(); }
        catch { }
    }

    public virtual void Enter()
    {
        rb = player.rb;
        player.anim.SetBool(animBoolName, true);
    }

    public virtual void Update()
    {
        
        xInput = Input.GetAxisRaw("Horizontal");
        
        player.anim.SetFloat("yVelocity", rb.linearVelocityY);
    }

    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
    }
}
