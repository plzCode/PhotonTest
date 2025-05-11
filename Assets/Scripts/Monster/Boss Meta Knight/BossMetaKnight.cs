using System.Xml;
using Photon.Pun;
using UnityEngine;

public class BossMetaKnight : Enemy
{
    #region States

    public MetaKnight_Idle idleState { get; private set; }
    public MetaKnight_Move moveState { get; private set; }
    public MetaKnight_Dash DashState { get; private set; }
    public MetaKnight_Jump jumpState { get; private set; }
    public MetaKnight_AirJump airJumpState { get; private set; }
    public MetaKnight_Attack1 attack1State { get; private set; }
    public MetaKnight_Attack2 attack2State { get; private set; }
    public MetaKnight_Attack3 attack3State { get; private set; }
    public MetaKnight_Attack4 attack4State { get; private set; }
    public MetaKnight_Attack5 attack5State { get; private set; }
    public MetaKnight_Dead deadState { get; private set; }
    public MetaKnight_Ready readyState { get; private set; }
    public MetaKnight_UnReady unReadyState { get; private set; }


    [Header("체력 UI")]
    [SerializeField] private Monster_HealthBar health_Bar;

    #endregion

    [Header("이펙트 정보")]
    public GameObject AirPrefab;
    public Transform AirPos;


    [Header("공격 정보")]
    [SerializeField] private GameObject Attack1Prefab;
    [SerializeField] private Transform Attack1Pos;
    [SerializeField] private GameObject Attack2Prefab;
    [SerializeField] private GameObject Attack3rightPrefab;
    [SerializeField] private GameObject Attack3leftPrefab;

    public static BossMetaKnight Instance;

    protected override void Awake()
    {
        base.Awake();
        Instance = this;

        idleState = new MetaKnight_Idle(this, stateMachine, "Idle");
        moveState = new MetaKnight_Move(this, stateMachine, "Move");
        DashState = new MetaKnight_Dash(this, stateMachine, "Dash");
        jumpState = new MetaKnight_Jump(this, stateMachine, "Jump");
        airJumpState = new MetaKnight_AirJump(this, stateMachine, "Air_Jump");
        attack1State = new MetaKnight_Attack1(this, stateMachine, "Attack1");
        attack2State = new MetaKnight_Attack2(this, stateMachine, "Attack2");
        attack3State = new MetaKnight_Attack3(this, stateMachine, "Attack3");
        attack4State = new MetaKnight_Attack4(this, stateMachine, "Attack4");
        attack5State = new MetaKnight_Attack5(this, stateMachine, "Attack5");
        deadState = new MetaKnight_Dead(this, stateMachine, "Dead");
        readyState = new MetaKnight_Ready(this, stateMachine, "Ready");
        unReadyState = new MetaKnight_UnReady(this, stateMachine, "UnReady");
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(readyState);

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
        if (currentHp <= 0)
        {
            stateMachine.ChangeState(deadState);
        }

    }

    [PunRPC]
    public void ChangeState(string stateName)
    {
        if (stateName == "Idle")
            stateMachine.ChangeState(idleState);
        else if (stateName == "Move")
            stateMachine.ChangeState(moveState);
        else if (stateName == "Dash")
            stateMachine.ChangeState(DashState);
        else if (stateName == "Jump")
            stateMachine.ChangeState(jumpState);
        else if (stateName == "Air_Jump")
            stateMachine.ChangeState(airJumpState);
        else if (stateName == "Attack1")
            stateMachine.ChangeState(attack1State);
        else if (stateName == "Attack2")
            stateMachine.ChangeState(attack2State);
        else if (stateName == "Attack3")
            stateMachine.ChangeState(attack3State);
        else if (stateName == "Attack4")
            stateMachine.ChangeState(attack4State);
        else if (stateName == "Attack5")
            stateMachine.ChangeState(attack5State);
        else if (stateName == "Dead")
            stateMachine.ChangeState(deadState);
        else if (stateName == "Ready")
            stateMachine.ChangeState(readyState);
        else if (stateName == "UnReady")
            stateMachine.ChangeState(unReadyState);

    }

    [PunRPC]
    public void Attack1RPC()
    {

        if (PhotonNetwork.IsMasterClient)
        {
            GameObject _currentBomb = PhotonNetwork.Instantiate("Monster_Effect/" + Attack1Prefab.name, Attack1Pos.position, Quaternion.identity);
        }
    }

    [PunRPC]
    public void Attack2RPC()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-3f, 3f), 0f, 0f);
            Vector3 spawnPos = transform.position + randomOffset;

            GameObject _currentBomb = PhotonNetwork.Instantiate("Monster_Effect/" + Attack2Prefab.name, spawnPos, Quaternion.identity);
        }
    }

    [PunRPC]
    public void Attack3RPC()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GameObject _currentBomb = PhotonNetwork.Instantiate("Monster_Effect/" + Attack3leftPrefab.name, transform.position, Quaternion.identity);
            GameObject _currentBomb1 = PhotonNetwork.Instantiate("Monster_Effect/" + Attack3rightPrefab.name, transform.position, Quaternion.identity);
        }
    }


    [PunRPC]
    public void ChangeAnimInteger(string _integerName, int _value)
    {
        anim.SetInteger(_integerName, _value);
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
