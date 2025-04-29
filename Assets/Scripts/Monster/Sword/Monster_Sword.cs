using Photon.Pun;
using UnityEngine;

public class Monster_Sword : Enemy
{
    [Header("공격 대쉬")]
    [SerializeField] private float dashSpeed;
    

    #region States
    public M_Sword_MoveState moveState { get; private set; }
    public M_Sword_AttackState attackState { get; private set; }

    #endregion
    protected override void Awake()
    {
        base.Awake();

        moveState = new M_Sword_MoveState(this, stateMachine, "Move", this);
        attackState = new M_Sword_AttackState(this, stateMachine, "Attack", this);
    }

    protected override void Start()
    {
        base.Start();
                
        stateMachine.Initialize(moveState);
    }

    protected override void Update()
    {
        base.Update();
    }

    [PunRPC]
    public void ChangeState(string stateName)
    {
        if (stateName == "Move")
            stateMachine.ChangeState(moveState);
        else if (stateName == "Attack")
            stateMachine.ChangeState(attackState);            
        
    }
    [PunRPC]
    public void RequestAnimIntegerChange(string _boolName,int _integer)
    {
        anim.SetInteger(_boolName, _integer);
    }

    [PunRPC]
    public void RequestAttackFromClient()
    {

        stateMachine.ChangeState(attackState);


    }

    [PunRPC]
    public void RequestHitFromClient()
    {
        //stateMachine.ChangeState(hitState);
    }

    [PunRPC]
    public void RequestMoveFromClient()
    {
        stateMachine.ChangeState(moveState);

    }


    public void Sword_AttackForward()
    {
        SetVelocity(dashSpeed * facingDir, rb.linearVelocity.y);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector3(transform.position.x,transform.position.y+0.03f), new Vector3(transform.position.x + attackDistance * facingDir, transform.position.y+0.03f));
    }
}
