using Photon.Pun;
using System.Collections;
using System.Linq;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }

    #endregion
    [SerializeField] protected LayerMask whatIsPlayer;
    [Header("체력 정보")]
    [SerializeField] public float maxHp=10;
    [SerializeField] public float currentHp=10;
    [SerializeField] public GameObject dieEffect;
    [SerializeField] protected SpriteRenderer spriteRenderer;  // 스프라이트 렌더러 참조
    private Coroutine hitFlashRoutine;

    [Header("넉백 정보")]
    [SerializeField] protected Vector2 knockbackDirection;
    [SerializeField] protected float knockbackDuration;
    protected bool isKnocked;


    [Header("충돌 정보")]
    public Transform attackCheck;
    public float attackCheckRadius;
    public Transform attack1Check;
    public float attack1CheckRadius;


    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;

    [Header("이동 정보")]
    public float moveSpeed = 10f;
    public float idleTime;
    public float battleTime;
    private float defaultMoveSpeed;

    [Header("공격 정보")]
    public bool canAttacking;
    public float attackDistance;
    public float attackCooldown;
    public float attackPower;
    [HideInInspector] public float lastTimeAttacked;

    


    public int facingDir= 1;
    public bool facingRight = true;
    public bool isJump = false;

    [Header("피해 정보")]
    
    public Vector2 stunDirection;
    public float hitTime;
    public EnemyFX fx { get; private set; }

    [Header("시작 정보")]
    [SerializeField] protected bool startRight = true;
    [SerializeField] protected Vector3 startPosition;
    [SerializeField] protected bool isFirstSpawn = true;
    public MonsterSpawner monsterSpawner;

    public EnemyStateMachine stateMachine { get; private set; }

    

    //For Photon
    public PhotonView photonView; 
    private Vector3 targetPosition;
    private Quaternion targetRotation;

    //For Boss Spawn Event
    [Header("몬스터 상태")]
    public bool isBusy = false;
    public bool stateMachineInitialized { get; private set; } = false;


    protected virtual void Awake()
    {
        stateMachine = new EnemyStateMachine();
        defaultMoveSpeed = moveSpeed;
        if (!startRight)
        {
            Flip();
        }
        photonView = GetComponent<PhotonView>();
        currentHp = maxHp;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        stateMachineInitialized = false;
    }

    protected virtual void Start()
    {
        //fx = GetComponent<EntityFX>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        fx = GetComponent<EnemyFX>();
        startPosition = transform.position;
        isFirstSpawn = false;

        stateMachineInitialized = true;
    }


    protected virtual void Update()
    {
        if (isBusy)
            return;
        stateMachine.currentState.Update();

        if(this.HasParameter("yVelocity", anim))
            anim.SetFloat("yVelocity", rb.linearVelocityY);

        /*if (PhotonNetwork.IsMasterClient)
        {
            // 마스터 클라이언트에서 몬스터 상태 업데이트
            targetPosition = transform.position + Vector3.left * Time.deltaTime; // 예: 왼쪽으로 이동
            targetRotation = transform.rotation;

            // 다른 클라이언트에 상태 전파
            photonView.RPC("SyncMonsterState", RpcTarget.Others, targetPosition, targetRotation);
        }
        else
        {
            // 다른 클라이언트에서 상태 적용
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 10);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10);
        }*/
    }
    // 몬스터 상태 동기화
    [PunRPC] 
    protected void SyncMonsterState(Vector3 position, Quaternion rotation)
    {
        targetPosition = position;
        targetRotation = rotation;
    }

    public void setStart()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    [PunRPC]
    public virtual void Die()
    {
        EffectAdd("Delete Enemy Effect 45x45_0", transform.position);
        if (!PhotonNetwork.IsMasterClient) return;

        if (this is Boss_Bonkers)
        {
            AudioManager.Instance.StopBGM();
            StartCoroutine(DieTime());
            SpawnReward();
            return;
        }
        else if (this is Boss_DDD)
        {
            StartCoroutine(DieTime());
            SpawnReward();
            return;
        }
        else if (this is BossMetaKnight)
        {
            StartCoroutine(DieTime());
            SpawnReward();
            return;
        }
        //if (PhotonNetwork.IsMasterClient)
        //{
        //    //isDestroyed = true;  // 삭제 상태로 설정
        //    photonView.RPC("EffectAdd", RpcTarget.All, "Delete Enemy Effect 45x45_0", transform.position);
        //    //PhotonNetwork.Destroy(gameObject); // 네트워크 전체에서 삭제
        //}EffectAdd("Delete Enemy Effect 45x45_0", transform.position);

    }
    [PunRPC]
    protected virtual void EffectAdd(string effectName, Vector3 effectPos)
    {
        PhotonNetwork.Instantiate("Monster_Effect/" + effectName, effectPos, Quaternion.identity);        
    }
    public virtual void Damage()
    {
        //fx.StartCoroutine("FlashFX");
        StartCoroutine("HitKnockBack");
        Debug.Log(gameObject.name + "데미지를 입혔다.");
    }

    [PunRPC]
    public virtual void TakeDamage(float _damage)
    {
        AudioManager.Instance.RPC_PlaySFX("Enemy_Damage");

        if (currentHp>0)
        {
            // 색상 변화 효과 시작
            if (hitFlashRoutine != null)
                StopCoroutine(hitFlashRoutine);
            hitFlashRoutine = StartCoroutine(HitFlash());
        }
        
        //stateMachine.ChangeState();

    }

    private IEnumerator HitFlash()
    {
        if (spriteRenderer == null)
            yield break;

        for (int i = 0; i < 2; i++)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
    }


    protected virtual IEnumerator HitKnockBack()
    {
        isKnocked = true;

        rb.linearVelocity = new Vector2(knockbackDirection.x * -facingDir, knockbackDirection.y);

        yield return new WaitForSeconds(knockbackDuration);

        isKnocked = false;

    }
    public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, 50, whatIsPlayer);

    #region 충돌
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);


    protected virtual void OnDrawGizmos()
    {

        if (groundCheck != null)
            Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        if (wallCheck != null)
            Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance * facingDir, wallCheck.position.y));
        if (attackCheck != null)
            Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
        if (attack1Check != null)
            Gizmos.DrawWireSphere(attack1Check.position, attack1CheckRadius);
    }

    // 파라미터를 가지고있는지 확인한다.
    public bool HasParameter(string paramName, Animator animator)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == paramName)
                return true;
        }
        return false;
    }
    #endregion

    #region 플립
    public virtual void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    [PunRPC]
    public virtual void FlipRPC()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }


    /*
    public void IsRightRotation(bool _Right)
    {
        int right = 0;
        if(_Right)
        {
            right = 1;
        }
        else
        {
            right = -1;
        }

        facingDir = right;
        facingRight = _Right;
        transform.rotation = Quaternion.Euler(0, 90 - (90 * right), 0);        
    }
    */



    public virtual void FlipController(float _x)
    {
        if (_x > 0 && !facingRight)
            Flip();
        else if (_x < 0 && facingRight)
            Flip();

    }

    #endregion


    #region 속력
    public void SetZeroVelocity()
    {
        if (isKnocked)
            return;
        rb.linearVelocity = new Vector2(0, 0);
    }
    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        if (isKnocked)
            return;


        rb.linearVelocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }
    #endregion

    public void Call_RPC(string name, RpcTarget type) 
    {
        photonView.RPC(name, type);
    }

    #region 삭제요청    
    [PunRPC]
    public void DestroySelf()
    {
        PhotonNetwork.Destroy(gameObject);
    }
    #endregion

    [PunRPC]
    public void SyncStateTimer(float timerValue)
    {
        // stateTimer를 동기화 처리
        if(stateMachine.currentState != null)
            stateMachine.currentState.stateTimer = timerValue;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.collider.CompareTag("Player") && PhotonNetwork.IsMasterClient)
        {
            Debug.Log("플레이어에게 " + 10f + "만큼 데미지를 줍니다.");

            PhotonView targetView = collision.collider.GetComponent<PhotonView>();
            if (targetView != null)
            {
                targetView.RPC("TakeDamage", RpcTarget.All, (Vector2)transform.position, 10f);
            }

        }
    }

    [PunRPC]
    public void WaitAndAction(float time)
    {
        StartCoroutine(WaitAndActionCoroutine(time));
    }

    private IEnumerator WaitAndActionCoroutine(float time)
    {
        // 초기화가 완료될 때까지 대기
        while (!stateMachineInitialized)
        {
            Debug.LogWarning("StateMachine is not initialized yet. Waiting...");
            yield return null; // 다음 프레임까지 대기
        }

        // 초기화 완료 후 동작 실행
        StartCoroutine(BusyFor(time));

        
    }

    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;
        if (PhotonNetwork.IsMasterClient)
        {
            if (this.gameObject.GetComponent<Boss_Bonkers>() != null)
            {
                Debug.Log("JUMP");
                //this.gameObject.GetComponent<Boss_Bonkers>().
                this.gameObject.GetComponent<PhotonView>().RPC("ChangeState", RpcTarget.AllBuffered, "Jump");
            }
        }
        yield return new WaitForSeconds(_seconds);

        if (PhotonNetwork.IsMasterClient)
        {
            if (this.gameObject.GetComponent<Boss_Bonkers>() != null)
            {
                this.gameObject.GetComponent<Boss_Bonkers>().forEventInit();
            }
            if (this.gameObject.GetComponent<Boss_DDD>() != null)
            {
                this.gameObject.GetComponent<Boss_DDD>().forEventInit();
            }
            if (this.gameObject.GetComponent<BossMetaKnight>() != null)
            {
                this.gameObject.GetComponent<BossMetaKnight>().forEventInit();
            }
        }
        isBusy = false;
    }
    IEnumerator DieTime()
    {
        
        int repeatCount = 5;

        for (int i = 0; i < repeatCount; i++)
        {
            AudioManager.Instance.PlaySFX("Boss_Die"); // 원하는 사운드 이름으로 교체
            yield return new WaitForSeconds(0.2f);
        }

        PhotonNetwork.Destroy(gameObject);
    }
    public void SpawnReward()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // 보상 스폰 로직
            GameObject reward = PhotonNetwork.Instantiate("Item/Reward/Chest", transform.position, Quaternion.identity);

            // 추가적인 보상 설정이나 초기화 작업 수행
        }
    }
}
