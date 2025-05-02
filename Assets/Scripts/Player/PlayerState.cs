using Photon.Realtime;
using UnityEngine;
using UnityEngine.Windows;
using Input = UnityEngine.Input;
using Photon.Pun;

public class PlayerState
{
    protected Player player;
    protected PlayerStateMachine stateMachine;

    public float xInput;

    protected Rigidbody2D rb;

    private string animBoolName;

    protected bool triggerCalled;

    //for Photon
    protected PhotonView pView;

    
    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
        pView = _player.GetComponent<PhotonView>();
    }

    public virtual void Enter()
    {
        triggerCalled = false;
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
    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
