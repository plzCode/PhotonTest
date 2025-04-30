using Photon.Pun;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }

    #endregion
    [SerializeField] protected LayerMask whatIsPlayer;


    [Header("넉백 정보")]
    [SerializeField] protected Vector2 knockbackDirection;
    [SerializeField] protected float knockbackDuration;
    protected bool isKnocked;


    [Header("충돌 정보")]
    public Transform attackCheck;
    public float attackCheckRadius;


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

    [Header("피해 정보")]
    
    public Vector2 stunDirection;
    public float hitTime;
    public EnemyFX fx { get; private set; }

    [Header("시작 정보")]
    [SerializeField] private bool startRight = true;


    public EnemyStateMachine stateMachine { get; private set; }

    

    //For Photon
    public PhotonView photonView; 
    private Vector3 targetPosition;
    private Quaternion targetRotation;

    protected virtual void Awake()
    {
        stateMachine = new EnemyStateMachine();
        defaultMoveSpeed = moveSpeed;
        if (!startRight)
        {
            Flip();
        }
        photonView = GetComponent<PhotonView>();
    }

    protected virtual void Start()
    {
        //fx = GetComponent<EntityFX>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        fx = GetComponent<EnemyFX>();
    }


    protected virtual void Update()
    {
        stateMachine.currentState.Update();

        if (PhotonNetwork.IsMasterClient)
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
        }
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



    public virtual void Damage()
    {
        //fx.StartCoroutine("FlashFX");
        StartCoroutine("HitKnockBack");
        Debug.Log(gameObject.name + "데미지를 입혔다.");
    }

    [PunRPC]
    public virtual void TakeDamage(float _damage)
    {
        //stateMachine.ChangeState();

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
}
