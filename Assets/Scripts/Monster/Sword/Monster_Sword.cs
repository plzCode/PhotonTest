using Photon.Pun;
using UnityEngine;

public class Monster_Sword : Enemy
{
    [Header("공격 대쉬")]
    [SerializeField] private float dashSpeed;
    

    #region States
    public M_Sword_MoveState moveState { get; private set; }
    public M_Sword_AttackState attackState { get; private set; }
    public Spear_HitState hitState { get; private set; }

    #endregion
    protected override void Awake()
    {
        base.Awake();

        moveState = new M_Sword_MoveState(this, stateMachine, "Move");
        attackState = new M_Sword_AttackState(this, stateMachine, "Attack");
        hitState = new Spear_HitState(this, stateMachine, "Hit");
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
        if (stateName == "Idle" || stateName == "Move")
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
        stateMachine.ChangeState(hitState);
    }

    [PunRPC]
    public void RequestMoveFromClient()
    {
        stateMachine.ChangeState(moveState);

    }

    [PunRPC]
    public override void TakeDamage(float _damage)
    {
        base.TakeDamage(_damage);
        Debug.Log("몬스터가 피해를 " + _damage + "받음");

        photonView.RPC("RequestHitFromClient", RpcTarget.All);
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
