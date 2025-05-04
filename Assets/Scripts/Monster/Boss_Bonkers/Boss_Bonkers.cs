using Photon.Pun;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Boss_Bonkers : Enemy
{
    #region States

    public Bonkers_IdleState idleState { get; private set; }
    public Bonkers_MoveState moveState { get; private set; }
    public Bonkers_JumpState jumpState { get; private set; }



    #endregion


    protected override void Awake()
    {
        base.Awake();

        idleState = new Bonkers_IdleState(this, stateMachine, "Idle");
        moveState = new Bonkers_MoveState(this, stateMachine, "Move");
        jumpState = new Bonkers_JumpState(this, stateMachine, "Jump");

    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
    }

    [PunRPC]
    public void ChangeState(string stateName)
    {
        if (stateName == "Idle")
            stateMachine.ChangeState(idleState);
        else if (stateName == "Move")
            stateMachine.ChangeState(moveState);
        else if (stateName == "Jump")
            stateMachine.ChangeState(jumpState);

    }

    [PunRPC]
    public void ChangeAnimInteger(string _integerName,int _value)
    {
        anim.SetInteger(_integerName, _value);
    }

    [PunRPC]
    public void SetJumpEnd()
    {
        anim.SetBool("JumpEnd", true);
    }

    
}
