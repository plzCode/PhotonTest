using Photon.Pun;
using UnityEngine;

public class Boss_DDD : Enemy
{
    #region States

    public DDD_IdleState idleState { get; private set; }
    public DDD_MoveState moveState { get; private set; }
    public DDD_JumpState jumpState { get; private set; }
    public DDD_AirJumpState airJumpState { get; private set; }
    public DDD_AirJumpingState airJumpingState { get; private set; }
    public DDD_AttackState attackState { get; private set; }
    public DDD_Attack1State attack1State { get; private set; }
    public DDD_Attack2State attack2State { get; private set; }
    public DDD_Attack3State attack3State { get; private set; }
    public DDD_DieState dieState { get; private set; }

    [Header("ü�� UI")]
    [SerializeField] private Monster_HealthBar health_Bar;

    #endregion

    [Header("��ź ����")]
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private Transform bombStart;


    protected override void Awake()
    {
        base.Awake();

        idleState = new DDD_IdleState(this, stateMachine, "Idle");
        moveState = new DDD_MoveState(this, stateMachine, "Move");
        jumpState = new DDD_JumpState(this, stateMachine, "Jump");
        airJumpState = new DDD_AirJumpState(this, stateMachine, "Air_Jump");
        airJumpingState = new DDD_AirJumpingState(this, stateMachine, "Air_Jumping");
        attackState = new DDD_AttackState(this, stateMachine, "Attack");
        attack1State = new DDD_Attack1State(this, stateMachine, "Attack1");
        attack2State = new DDD_Attack2State(this, stateMachine, "Attack2");
        attack3State = new DDD_Attack3State(this, stateMachine, "Attack3");
        dieState = new DDD_DieState(this, stateMachine, "Die");

    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);

        UpdateCurrentPlayersCollision();

        // �÷��̾�� ������ �ݶ��̴��� ã�Ƽ� �浹�� ����
        //Collider2D playerCollider = GameObject.Find("Player(Clone)").GetComponent<Collider2D>();
        //Collider2D monsterCollider = GetComponent<Collider2D>();

        //if (playerCollider != null && monsterCollider != null)
        //{
        //    Debug.Log("�浹���ü���");
        //    Physics2D.IgnoreCollision(playerCollider, monsterCollider, true);  // ������ �浹 ����
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
            Debug.LogWarning("Monster Collider�� �����ϴ�.");
            return;
        }

        // �÷��̾� �迭�� ��ȿ���� Ȯ��
        if (GameManager.Instance.playerList == null || GameManager.Instance.playerList.Count == 0)
        {
            Debug.LogWarning("�÷��̾� �迭�� ����ֽ��ϴ�.");
            return;
        }

        // �÷��̾�� ������ �ݶ��̴��� ã�Ƽ� �浹�� ����
        for (int i = 0; i < GameManager.Instance.playerList.Count; i++)
        {
            // �� �÷��̾��� Collider2D�� �ִ��� Ȯ��
            Collider2D playerCollider = GameManager.Instance.playerList[i].GetComponent<Collider2D>();
            if (playerCollider != null)
            {
                Physics2D.IgnoreCollision(playerCollider, monsterCollider, true);  // ������ �浹 ����
                Debug.Log("�÷��̾� " + i + "�� ������ �浹 ���� ����");
            }
            else
            {
                Debug.LogWarning("�÷��̾� " + i + "�� Collider2D�� �����ϴ�.");
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
        Debug.Log("���Ͱ� ���ظ� " + _damage + "����");


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
    public void ChangeAnimInteger(string _integerName, int _value)
    {
        anim.SetInteger(_integerName, _value);
    }

    [PunRPC]
    public void SetJumpEnd()
    {
        anim.SetBool("JumpEnd", true);
    }

    [PunRPC]
    public void AirOutRPC()
    {

        if (PhotonNetwork.IsMasterClient)
        {
            //Debug.Log("ThrowSpear RPC ȣ��� - IsMasterClient: " + PhotonNetwork.IsMasterClient);
            GameObject _currentBomb = PhotonNetwork.Instantiate("Monster_Effect/" + bombPrefab.name, bombStart.position, Quaternion.identity);
            //Rigidbody2D _BombRidig = _currentBomb.GetComponent<Rigidbody2D>();
            //_BombRidig.linearVelocity = new Vector2(10 * facingDir, 10);
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.CompareTag("Player") && PhotonNetwork.IsMasterClient)
        {
            Debug.Log("�÷��̾�� " + attackPower + "��ŭ �������� �ݴϴ�.");
            if (collision.GetComponent<PhotonView>() != null)
            {
                collision.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, (Vector2)transform.position, attackPower); // ������ ó��
            }



        }
    }
}
