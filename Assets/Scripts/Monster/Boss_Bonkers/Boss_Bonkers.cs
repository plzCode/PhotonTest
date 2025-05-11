using Photon.Pun;
using System.Collections;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Boss_Bonkers : Enemy
{
    #region States

    public Bonkers_IdleState idleState { get; private set; }
    public Bonkers_MoveState moveState { get; private set; }
    public Bonkers_JumpState jumpState { get; private set; }
    public Bonkers_AttackState attackState { get; private set; }
    public Bonkers_DieState dieState { get; private set; }

    [Header("체력 UI")]
    [SerializeField] private Monster_HealthBar health_Bar;

    #endregion

    [Header("폭탄 정보")]
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private Transform bombStart;


    protected override void Awake()
    {
        base.Awake();

        idleState = new Bonkers_IdleState(this, stateMachine, "Idle");
        moveState = new Bonkers_MoveState(this, stateMachine, "Move");
        jumpState = new Bonkers_JumpState(this, stateMachine, "Jump");
        attackState = new Bonkers_AttackState(this, stateMachine, "Attack");
        dieState = new Bonkers_DieState(this, stateMachine, "Die");

        

    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);

        UpdateCurrentPlayersCollision();

        // 플레이어와 몬스터의 콜라이더를 찾아서 충돌을 무시
        //Collider2D playerCollider = GameObject.Find("Player(Clone)").GetComponent<Collider2D>();
        //Collider2D monsterCollider = GetComponent<Collider2D>();

        //if (playerCollider != null && monsterCollider != null)
        //{
        //    Debug.Log("충돌무시설정");
        //    Physics2D.IgnoreCollision(playerCollider, monsterCollider, true);  // 물리적 충돌 무시
        //}


    }

    protected override void Update()
    {
        
        base.Update();

        //UpdateCurrentPlayersCollision();
    }

    //GameObject bonkersObject = GameObject.Find("Bonkers");
    //    if(bonkersObject!=null)
    //    {
    //        Boss_Bonkers bonkers = bonkersObject.GetComponent<Boss_Bonkers>();
    //bonkers.UpdateCurrentPlayersCollision();
    //    }

    public void UpdateCurrentPlayersCollision()
    {

        Collider2D monsterCollider = GetComponent<Collider2D>();
        if (monsterCollider == null)
        {
            Debug.LogWarning("Monster Collider가 없습니다.");
            return;
        }

        // 플레이어 배열이 유효한지 확인
        if (GameManager.Instance.playerList == null || GameManager.Instance.playerList.Count == 0)
        {
            Debug.LogWarning("플레이어 배열이 비어있습니다.");
            return;
        }

        // 플레이어와 몬스터의 콜라이더를 찾아서 충돌을 무시
        for (int i = 0; i < GameManager.Instance.playerList.Count; i++)
        {
            // 각 플레이어의 Collider2D가 있는지 확인
            Collider2D playerCollider = GameManager.Instance.playerList[i].GetComponent<Collider2D>();
            if (playerCollider != null)
            {
                Physics2D.IgnoreCollision(playerCollider, monsterCollider, true);  // 물리적 충돌 무시
                Debug.Log("플레이어 " + i + "와 몬스터의 충돌 무시 설정");
            }
            else
            {
                Debug.LogWarning("플레이어 " + i + "에 Collider2D가 없습니다.");
            }
        }
    }

    [PunRPC]
    public override void TakeDamage(float _damage)
    {
        base.TakeDamage(_damage);
        if (currentHp <= 0)
            return;
        currentHp -= _damage;
        health_Bar.UpdateHealthBar(maxHp, currentHp);
        Debug.Log("몬스터가 피해를 " + _damage + "받음");

        if(currentHp <= 0)
        {
            //health_Bar.UpdateHealthBar(maxHp, 0);
            // 몬스터 사망 처리
            stateMachine.ChangeState(dieState);
            //Destroy(gameObject);
        }
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
        else if (stateName == "Die")
            stateMachine.ChangeState(dieState);
        else
            Debug.LogError("Invalid state name: " + stateName);

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
            //Debug.Log("ThrowSpear RPC 호출됨 - IsMasterClient: " + PhotonNetwork.IsMasterClient);
            GameObject _currentBomb = PhotonNetwork.Instantiate("Monster_Effect/" + bombPrefab.name, bombStart.position, Quaternion.identity);
            //Rigidbody2D _BombRidig = _currentBomb.GetComponent<Rigidbody2D>();
            //_BombRidig.linearVelocity = new Vector2(10 * facingDir, 10);
        }
    }

    
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        
        if (collision.CompareTag("Player") && PhotonNetwork.IsMasterClient)
        {
            Debug.Log("플레이어에게 " + attackPower + "만큼 데미지를 줍니다.");
            if (collision.GetComponent<PhotonView>() != null)
            {
                collision.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, (Vector2)transform.position, attackPower); // 데미지 처리
            }
                
            
            
        }
    }

    private void OnEnable()
    {
        UpdateCurrentPlayersCollision();
    }


    //Tmp
    public void forEventInit()
    {

        stateMachine.Initialize(idleState);

        UpdateCurrentPlayersCollision();
    }

    
}
