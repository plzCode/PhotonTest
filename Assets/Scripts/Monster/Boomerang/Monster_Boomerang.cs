using Photon.Pun;
using UnityEngine;

public class Monster_Boomerang : Enemy
{
    [Header("부메랑 관련(플레이어 탐지 거리,프리팹)")]
    [SerializeField] public float playerCheckDistance;
    [SerializeField] private GameObject boomerangPrefab;

    public M_Boomerang_IdleState idleState{ get; private set; }
    public M_Sword_MoveState moveState { get; private set; }
    public M_Sword_AttackState attackState { get; private set; }
    public Spear_HitState hitState { get; private set; }


    private GameObject currentBoomerang;
    private Boomerang_Obj boomerang_Script; 

    protected override void Awake()
    {
        base.Awake();

        idleState = new M_Boomerang_IdleState(this, stateMachine, "Idle", this);
        moveState = new M_Sword_MoveState(this, stateMachine, "Move");
        attackState = new M_Sword_AttackState(this, stateMachine, "Attack");
        hitState = new Spear_HitState(this, stateMachine, "Hit");
    }

    protected override void Start()
    {
        setStart();
        

        stateMachine.Initialize(idleState);
    }

    [PunRPC]
    public void ChangeState(string stateName)
    {
        if (stateName == "Idle")
            stateMachine.ChangeState(idleState);
        else if (stateName == "Move")
            stateMachine.ChangeState(moveState);
        else if (stateName == "Attack")
            stateMachine.ChangeState(attackState);
        else if (stateName == "Hit")
            stateMachine.ChangeState(hitState);
        else if (stateName == "ReSpawn")
        {
            MonsterSpawner.Instance.StartCoroutine(MonsterSpawner.Instance.ReSpawner(gameObject));
        }

    }


    [PunRPC]
    public override void TakeDamage(float _damage)
    {
        base.TakeDamage(_damage);
        if (currentHp <= 0)
        {
            return;
        }
        Debug.Log("몬스터가 피해를 " + _damage + "받음");
        currentHp -= _damage;
        if (currentHp <= 0)
        { currentHp = 0; }
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("ChangeState", RpcTarget.All, "Hit");
        }

    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector3(transform.position.x, transform.position.y + 0.03f), new Vector3(transform.position.x + attackDistance * facingDir, transform.position.y + 0.03f));
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerCheckDistance); // 탐지 사거리
        
    }

    [PunRPC]
    public void PickupBoomerang()
    {
        // 마스터 클라이언트에서만 투창을 소환하도록 수정
        if (PhotonNetwork.IsMasterClient)
        {
            //Debug.Log("ThrowSpear RPC 호출됨 - IsMasterClient: " + PhotonNetwork.IsMasterClient);
            if(PhotonNetwork.IsMasterClient)
            currentBoomerang = PhotonNetwork.Instantiate("Monster_Effect/" + boomerangPrefab.name, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            boomerang_Script = currentBoomerang.GetComponent<Boomerang_Obj>();
            boomerang_Script.SetVelocity(0, 0);
            if (facingDir == -1)
            {
                currentBoomerang.transform.Rotate(0, 180, 0);
                boomerang_Script.facingDir = -1;
            }
        }
        //Instantiate(spearPrefab, transform.position, Quaternion.identity);
    }

    private void OnEnable()
    {
        if (startRight && facingDir == -1)
        {
            Flip();
        }
        else if (!startRight && facingDir == 1)
        {
            Flip();
        }

        if (!isFirstSpawn)
            transform.position = startPosition;
        currentHp = maxHp;
        stateMachine.Initialize(idleState);
    }

    public void ThrowBoomerang()
    {
        
        if (PhotonNetwork.IsMasterClient)
        {
            boomerang_Script.photonView.RPC("ThrowBoomerang", RpcTarget.All,boomerang_Script.facingDir);
        }        
        
    }

}
