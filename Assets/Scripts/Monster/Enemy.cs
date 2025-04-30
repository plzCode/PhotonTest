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


    [Header("�˹� ����")]
    [SerializeField] protected Vector2 knockbackDirection;
    [SerializeField] protected float knockbackDuration;
    protected bool isKnocked;


    [Header("�浹 ����")]
    public Transform attackCheck;
    public float attackCheckRadius;


    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;

    [Header("�̵� ����")]
    public float moveSpeed = 10f;
    public float idleTime;
    public float battleTime;
    private float defaultMoveSpeed;

    [Header("���� ����")]
    public bool canAttacking;
    public float attackDistance;
    public float attackCooldown;
    public float attackPower;
    [HideInInspector] public float lastTimeAttacked;

    


    public int facingDir= 1;
    public bool facingRight = true;

    [Header("���� ����")]
    
    public Vector2 stunDirection;
    public float hitTime;
    public EnemyFX fx { get; private set; }

    [Header("���� ����")]
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
            // ������ Ŭ���̾�Ʈ���� ���� ���� ������Ʈ
            targetPosition = transform.position + Vector3.left * Time.deltaTime; // ��: �������� �̵�
            targetRotation = transform.rotation;

            // �ٸ� Ŭ���̾�Ʈ�� ���� ����
            photonView.RPC("SyncMonsterState", RpcTarget.Others, targetPosition, targetRotation);
        }
        else
        {
            // �ٸ� Ŭ���̾�Ʈ���� ���� ����
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 10);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10);
        }
    }
    // ���� ���� ����ȭ
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
        Debug.Log(gameObject.name + "�������� ������.");
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

    #region �浹
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

    // �Ķ���͸� �������ִ��� Ȯ���Ѵ�.
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

    #region �ø�
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


    #region �ӷ�
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

    #region ������û    
    [PunRPC]
    public void DestroySelf()
    {
        PhotonNetwork.Destroy(gameObject);
    }
    #endregion

    [PunRPC]
    public void SyncStateTimer(float timerValue)
    {
        // stateTimer�� ����ȭ ó��
        if(stateMachine.currentState != null)
            stateMachine.currentState.stateTimer = timerValue;
    }
}
