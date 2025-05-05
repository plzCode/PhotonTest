using Photon.Pun;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Boss_Bonkers : Enemy
{
    #region States

    public Bonkers_IdleState idleState { get; private set; }
    public Bonkers_MoveState moveState { get; private set; }
    public Bonkers_JumpState jumpState { get; private set; }
    public Bonkers_AttackState attackState { get; private set; }



    #endregion

    [Header("ÆøÅº Á¤º¸")]
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private Transform bombStart;


    protected override void Awake()
    {
        base.Awake();

        idleState = new Bonkers_IdleState(this, stateMachine, "Idle");
        moveState = new Bonkers_MoveState(this, stateMachine, "Move");
        jumpState = new Bonkers_JumpState(this, stateMachine, "Jump");
        attackState = new Bonkers_AttackState(this, stateMachine, "Attack");

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
        else if (stateName == "Attack")
            stateMachine.ChangeState(attackState);

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

    [PunRPC]
    public void ThrowBombRPC()
    {
        
        if (PhotonNetwork.IsMasterClient)
        {
            //Debug.Log("ThrowSpear RPC È£ÃâµÊ - IsMasterClient: " + PhotonNetwork.IsMasterClient);
            GameObject _currentBomb = PhotonNetwork.Instantiate("Monster_Effect/" + bombPrefab.name, bombStart.position, Quaternion.identity);
            //Rigidbody2D _BombRidig = _currentBomb.GetComponent<Rigidbody2D>();
            //_BombRidig.linearVelocity = new Vector2(10 * facingDir, 10);
        }
    }
    
}
