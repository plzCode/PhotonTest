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
    [Header("ü�� ����")]
    [SerializeField] public float maxHp=10;
    [SerializeField] public float currentHp=10;
    [SerializeField] public GameObject dieEffect;
    [SerializeField] protected SpriteRenderer spriteRenderer;  // ��������Ʈ ������ ����
    private Coroutine hitFlashRoutine;

    [Header("�˹� ����")]
    [SerializeField] protected Vector2 knockbackDirection;
    [SerializeField] protected float knockbackDuration;
    protected bool isKnocked;


    [Header("�浹 ����")]
    public Transform attackCheck;
    public float attackCheckRadius;
    public Transform attack1Check;
    public float attack1CheckRadius;


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
    public bool isJump = false;

    [Header("���� ����")]
    
    public Vector2 stunDirection;
    public float hitTime;
    public EnemyFX fx { get; private set; }

    [Header("���� ����")]
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
    [Header("���� ����")]
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
        }*/
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
        //    //isDestroyed = true;  // ���� ���·� ����
        //    photonView.RPC("EffectAdd", RpcTarget.All, "Delete Enemy Effect 45x45_0", transform.position);
        //    //PhotonNetwork.Destroy(gameObject); // ��Ʈ��ũ ��ü���� ����
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
        Debug.Log(gameObject.name + "�������� ������.");
    }

    [PunRPC]
    public virtual void TakeDamage(float _damage)
    {
        AudioManager.Instance.RPC_PlaySFX("Enemy_Damage");

        if (currentHp>0)
        {
            // ���� ��ȭ ȿ�� ����
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
        if (attack1Check != null)
            Gizmos.DrawWireSphere(attack1Check.position, attack1CheckRadius);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.collider.CompareTag("Player") && PhotonNetwork.IsMasterClient)
        {
            Debug.Log("�÷��̾�� " + 10f + "��ŭ �������� �ݴϴ�.");

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
        // �ʱ�ȭ�� �Ϸ�� ������ ���
        while (!stateMachineInitialized)
        {
            Debug.LogWarning("StateMachine is not initialized yet. Waiting...");
            yield return null; // ���� �����ӱ��� ���
        }

        // �ʱ�ȭ �Ϸ� �� ���� ����
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
            AudioManager.Instance.PlaySFX("Boss_Die"); // ���ϴ� ���� �̸����� ��ü
            yield return new WaitForSeconds(0.2f);
        }

        PhotonNetwork.Destroy(gameObject);
    }
    public void SpawnReward()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // ���� ���� ����
            GameObject reward = PhotonNetwork.Instantiate("Item/Reward/Chest", transform.position, Quaternion.identity);

            // �߰����� ���� �����̳� �ʱ�ȭ �۾� ����
        }
    }
}
