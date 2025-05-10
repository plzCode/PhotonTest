using JetBrains.Annotations;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float PlayerHP;
    public float PlayerMaxHP = 100f; //�÷��̾� �ִ� ü��
    public int PlayerLife = 3;

    public float MoveSpeed;
    public float DashSpeed;

    public float JumpPower;

    public bool isBusy { get; private set; }

    public bool flipbool = true;
    public float LastMove;

    public bool TimeBool = true;

    public GameObject dashEffect;
    public Transform dashEffectPos;
    public GameObject AirJumpOutEffect;
    public Transform AirJumpOutEffectPos;
    public GameObject EatEffect1;
    public GameObject EatEffect2;
    public Transform EatEffectPos;
    public GameObject GroundStarEffect;
    public Transform GroundEffectPos;
    public GameObject DamageStar;

    public List<GameObject> AttackList = new List<GameObject>();
    public Collider2D currentEnemy;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundLine;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform hillCheck;
    [SerializeField] private float hillLine;
    [SerializeField] private LayerMask whatIsHill;

    public float angle;
    public Vector2 perp;
    public bool isSlope;

    [SerializeField] private SpriteRenderer spriteRenderer;


    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }

    #region ����
    public PlayerState playerState { get; private set; }
    public PlayerStateMachine stateMachine { get; private set; }

    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerDownState downState { get; private set; }
    public PlayerSlidingState slidingState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerDashTurnState dashTurnState { get; private set; }

    public PlayerJumpState jumpState { get; private set; }
    public PlayerDowningState downingState { get; private set; }
    public PlayerDowningGroundState downingGroundState { get; private set; }

    public PlayerAirState airState { get; private set; }
    public PlayerAirJumpState airJumpState { get; private set; }
    public PlayerAirJumpingState airJumpingState { get; private set; }
    public PlayerAirJumpUpState airJumpUpState { get; private set; }
    public PlayerAirJumpOutState airJumpOutState { get; private set; }

    public PlayerEating12State eating12State { get; private set; }
    public PlayerEating3State eating3State { get; private set; }
    public PlayerEating4State eating4State { get; private set; }
    public PlayerEatingEndState eatingEndState { get; private set; }

    public PlayerEatState eatState { get; private set; }

    public PlayerDamageState damageState { get; private set; }
    public PlayerDieState dieState { get; private set; }

    public PlayerChageFormState changeFormState { get; private set; }

    #endregion

    //For Test Ability
    public PlayerAbility curAbility;
    public bool isInhaling = false;
    [SerializeField]
    public Transform attackCheck;


    public PhotonView pView;

    //UI
    public Health_Bar health_Bar;
    //life
    public LifeNum lifeNum;
    //Inventory
    public Inventory inventory;
    //Command
    public CommandInput commandInput;


    #region AttackUpgrade

    public float CutterUpgrade;
    #endregion

    public void Awake()
    {
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        downState = new PlayerDownState(this, stateMachine, "Down");
        slidingState = new PlayerSlidingState(this, stateMachine, "Sliding");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        dashTurnState = new PlayerDashTurnState(this, stateMachine, "DashTurn");

        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        downingState = new PlayerDowningState(this, stateMachine, "Downing");
        downingGroundState = new PlayerDowningGroundState(this, stateMachine, "DowningGround");

        airState = new PlayerAirState(this, stateMachine, "Jump");
        airJumpState = new PlayerAirJumpState(this, stateMachine, "AirJump");
        airJumpingState = new PlayerAirJumpingState(this, stateMachine, "AirJumping");
        airJumpUpState = new PlayerAirJumpUpState(this, stateMachine, "AirJumpUp");
        airJumpOutState = new PlayerAirJumpOutState(this, stateMachine, "AirJumpOut");

        eating12State = new PlayerEating12State(this, stateMachine, "Eating12");
        eating3State = new PlayerEating3State(this, stateMachine, "Eating3");
        eating4State = new PlayerEating4State(this, stateMachine, "Eating4");
        eatingEndState = new PlayerEatingEndState(this, stateMachine, "EatingEnd");

        eatState = new PlayerEatState(this, stateMachine, "Eat");

        damageState = new PlayerDamageState(this, stateMachine, "Damage");
        dieState =new PlayerDieState(this, stateMachine, "Die");

        changeFormState = new PlayerChageFormState(this, stateMachine, "ChangeForm");


        pView = GetComponent<PhotonView>();
        commandInput = GetComponent<CommandInput>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (pView.IsMine)
        {
            PhotonNetwork.LocalPlayer.TagObject = gameObject; // 로컬 플레이어의 GameObject를 TagObject로 설정
        }
    }

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        stateMachine.StartState(idleState);

        LastMove = 1f; //�÷��̾� ������ �Է°��� �ʱ� 1�� ����
    }

    public void Update()
    {
        //Debug.Log(KirbyFormNum);
        stateMachine.state.Update();

        DashTime(); //�뽬��ȣ�ۿ� Ÿ��
        Hill();

        if (pView.IsMine == false) return; //�� ĳ���Ͱ� �ƴҶ��� ����        

        if (Input.GetKeyDown(KeyCode.G))
        {
            if (curAbility != null)
            {
                pView.RPC("DropAbility", RpcTarget.All, -1 * LastMove, DamageStar.name, transform.position);
            }

        }
    }
    [PunRPC]
    public void DropAbility(float _x, string Effect, Vector3 pos)
    {
        EffectAdd(_x, Effect, pos);
        curAbility.OnAbilityDestroyed(this);
        Destroy(curAbility);
    }



    [PunRPC]
    public void SyncFormNum(int EatKirbyFormNum)
    {
        this.EatKirbyFormNum = EatKirbyFormNum;
        KirbyFormNum = this.EatKirbyFormNum;
    }


    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;


        yield return new WaitForSeconds(_seconds);

        isBusy = false;
    }


    //�浹üũ
    public bool IsGroundCheck() => Physics2D.OverlapCircle(groundCheck.position, groundLine, whatIsGround | whatIsHill);
    public bool IsHillCheck() => Physics2D.Raycast(hillCheck.position, Vector2.down, hillLine, whatIsHill);

    public void Hill()
    {
        RaycastHit2D hit = Physics2D.Raycast(hillCheck.position, Vector2.down, hillLine, whatIsHill);

        if (hit)
        {
            perp = Vector2.Perpendicular(hit.normal).normalized;
            angle = Vector2.Angle(hit.normal, Vector2.up);
            isSlope = angle != 0;

            Debug.DrawLine(hit.point, hit.point + hit.normal, Color.blue);
            Debug.DrawLine(hit.point, hit.point + perp, Color.red);
        }
        else
        {
            isSlope = false; // ����� �ƴ� ���
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundLine);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(hillCheck.position, new Vector3(hillCheck.position.x, hillCheck.position.y - hillLine));
    }

    #region ĳ���� ������ �� �¿����
    //�̹��� �¿����
    public void Flip()
    {
        flipbool = !flipbool;
        transform.Rotate(0, 180, 0);
    }

    public void FlipController(float _x)
    {
        if (_x > 0 && !flipbool)
        {
            Flip();
            LastMove = 1f;
        }
        else if (_x < 0 && flipbool)
        {
            Flip();
            LastMove = -1f;
        }
    }

    //ĳ���� ������
    [PunRPC]
    public void lineVelocity(float xlineVelocity, float ylineVelocity)
    {
        if (rb == null) return;

        if (isSlope && IsHillCheck())
        {
            // �����̸�, ��� �������� �̵�
            rb.linearVelocity = perp * xlineVelocity * -1f;
            rb.linearVelocity = new Vector2(xlineVelocity, ylineVelocity);
        }
        else
        {
            // ������ �׳� �Ϲ� x�� �̵�
            rb.linearVelocity = new Vector2(xlineVelocity, ylineVelocity);
        }

        FlipController(xlineVelocity);
    }
    #endregion

    [PunRPC]
    public void EffectAdd(float _x, string Effect, Vector3 EffecPos) //����Ʈ�� �߰���
    {
        GameObject bullet = null;
        if (PhotonNetwork.IsMasterClient)
        {
            if (_x > 0) //�������̸� �״�� ��ȯ
            {
                //Instantiate(obj, EffecPos.position, Quaternion.identity);
                bullet = PhotonNetwork.Instantiate("Player_Effect/" + Effect, EffecPos, Quaternion.identity);
            

            }
            else if (_x < 0) //�����̸� �¿���� ��ȯ
            {
                //Instantiate(obj, EffecPos.position, Quaternion.Euler(0, 180, 0));
                bullet = PhotonNetwork.Instantiate("Player_Effect/" + Effect, EffecPos, Quaternion.Euler(0, 180, 0));

            }
        }

        if (bullet != null)
        {
            PlayerRagedManager attackScript = bullet.GetComponent<PlayerRagedManager>();
            if (attackScript != null)
            {
                attackScript.player = this; // �� �ڵ尡 Player Ŭ���� �ȿ� �־�� ��
            }
            if (bullet.GetComponent<KirbyDamageStar>() != null)
            {
                bullet.GetComponent<PhotonView>().RPC("SetPlayer", RpcTarget.AllBuffered, pView.ViewID);
            }

        }
    }

    [PunRPC]
    public void EffectForCutter(float _x, string Effect, Vector3 EffecPos)
    {
        GameObject bullet = null;
        if (pView.IsMine)
        {
            if (_x > 0) //�������̸� �״�� ��ȯ
            {
                //Instantiate(obj, EffecPos.position, Quaternion.identity);
                bullet = PhotonNetwork.Instantiate("Player_Effect/" + Effect, EffecPos, Quaternion.identity);


            }
            else if (_x < 0) //�����̸� �¿���� ��ȯ
            {
                //Instantiate(obj, EffecPos.position, Quaternion.Euler(0, 180, 0));
                bullet = PhotonNetwork.Instantiate("Player_Effect/" + Effect, EffecPos, Quaternion.Euler(0, 180, 0));

            }
        }

        if (bullet != null)
        {
            PlayerRagedManager attackScript = bullet.GetComponent<PlayerRagedManager>();
            if (attackScript != null)
            {
                attackScript.player = this; // �� �ڵ尡 Player Ŭ���� �ȿ� �־�� ��
            }
            if (bullet.GetComponent<KirbyDamageStar>() != null)
            {
                bullet.GetComponent<PhotonView>().RPC("SetPlayer", RpcTarget.AllBuffered, pView.ViewID);
            }

        }
    }
    /*[PunRPC]
    public void KirbyDamageStarSetting(int bView, int pView)
    {
        GameObject bullet = PhotonView.Find(bView).gameObject;
        Player player = PhotonView.Find(pView).gameObject.GetComponent<Player>();

        if (bullet.GetComponent<KirbyDamageStar>() != null)
        {
            bullet.GetComponent<KirbyDamageStar>().player = player; // �� �ڵ尡 Player Ŭ���� �ȿ� �־�� ��
            bullet.GetComponent<KirbyDamageStar>().enemyNumber.Number = player.KirbyFormNum;
        }
    }*/



    [PunRPC]
    public void AttackAdd(float _x, string effectName, Vector3 effectPos, int pViewId)
    {
        GameObject effect = null;
        PhotonView pView = PhotonView.Find(pViewId);

        if (_x > 0)
            effect = PhotonNetwork.Instantiate("Player_Effect/" + effectName, effectPos, Quaternion.identity);
        else if (_x < 0)
            effect = PhotonNetwork.Instantiate("Player_Effect/" + effectName, effectPos, Quaternion.Euler(0, 180, 0));

        if (effect != null)
        {
            if (effect.GetComponent<EatEffect>() != null)
            {
                effect.GetComponent<EatEffect>().player = pView.GetComponent<Player>();
                effect.GetComponent<EatEffect>().pView = pView;
            }
            effect.transform.SetParent(pView.transform);
            AttackList.Add(effect);
            //effect.GetComponent<PhotonView>().RPC("SetPlayer", RpcTarget.AllBuffered, pView.ViewID);
        }
    }


    [PunRPC]
    public void AttackDestroy()
    {
        foreach (GameObject effect in AttackList)
        {
            if (effect == null) continue;

            PhotonView view = effect.GetComponent<PhotonView>();
            if (view != null && view.IsMine)
            {
                PhotonNetwork.Destroy(effect);
            }
        }

        AttackList.Clear(); // ����Ʈ �ʱ�ȭ
    }

    [PunRPC]
    public void EatEnemy(int enemyViewId)
    {
        Debug.Log("EatEnemy");
        //if (enemy == null || this == null) return; //�̹� �԰��ִٸ� ����
        PhotonView eView = PhotonView.Find(enemyViewId);
        Debug.Log(enemyViewId);
        if (eView == null) return;
        Transform enemy = eView.transform;

        enemy.GetComponentInChildren<PhotonAnimatorView>().enabled = false; //�ִϸ����� ��Ȱ��ȭ
        enemy.GetComponent<PhotonView>().enabled = false; //����� ��Ȱ��ȭ
                                                          //PhotonView eView = enemy.GetComponent<PhotonView>();
                                                          //PhotonNetwork.Destroy(enemy.gameObject);  //������
                                                          //this.EatKirbyFormNum = PormNumber; //�Դ� Ŀ�� ��ǿ� ���� ���� ��ȣ ���� ����

        Debug.Log(enemy.name + " : " + eView.ViewID);
        this.EatKirbyFormNum = enemy.GetComponent<EnemyNumber>().Number;
        this.KirbyFormNum = 1; //�Դ� Ŀ��� ����
        
        this.stateMachine.ChangeState(this.eatState); //�÷��̾� �������            
                                                      //PlayerEetState���� ���� �Լ� ��

        //eView.RPC("DestroySelf", eView.Owner);
        if (enemy != null && PhotonNetwork.IsMasterClient)
        {
            Enemy Enemy = enemy.GetComponent<Enemy>();
            if (Enemy != null)
            {
                Enemy.DestroySelf();
            }
            else
            {
                Item Item = enemy.GetComponent<Item>();
                if (Item != null)
                {
                    Item.DestroySelf();
                }
            }

        }

    }


    public void SetCurrentTarget(Collider2D enemyCollider)
    {
        currentEnemy = enemyCollider;
    }


    [PunRPC]
    public void Change()
    {
        curAbility.OnAbilityDestroyed(this);
    }

    [PunRPC]
    public void TakeDamage(Vector2 EnemyAttackPos, float Damage)    //������ ���� ������ ����
    {
        if (isInvincible)
            return; //�������¸� ����

        

        if (transform.position.x > EnemyAttackPos.x)
        {
            LastMove = 1f;
        }
        else if (transform.position.x < EnemyAttackPos.x)
        {
            LastMove = -1f;
        }

        if (curAbility != null)
        {
            EffectAdd(LastMove, DamageStar.name, transform.position);            
            //pView.RPC("EffectAdd", RpcTarget.All, LastMove, DamageStar.name, transform.position);
            curAbility.OnAbilityDestroyed(this); //�����Ƽ �ʱ�ȭ
        }
        PlayerHP -= Damage;
        if(PlayerHP <= 0)
        {
            PlayerHP = 0;
            isBusy = true;
            Die_Player();
        }
        else
        {
            stateMachine.ChangeState(damageState);
        }
        if (health_Bar != null)
        {
            health_Bar.UpdateHealthBar(PlayerHP);
        }
        if (pView.IsMine)
        {
            AudioManager.Instance.RPC_PlaySFX("Damaged_Sound");
        }
          //�������°� ����
                
        //pView.RPC("RPC_StartNoDamage", RpcTarget.All, 2f, 0.2f);
        StartCoroutine(NoDamage(2f, 0.2f));
    }

    [PunRPC]
    public void TakeHeal(float HealAmount)
    {
        PlayerHP += HealAmount;
        if (PlayerHP > PlayerMaxHP)
            PlayerHP = PlayerMaxHP;
        if (health_Bar != null)
        {
            health_Bar.UpdateHealthBar(PlayerHP);
        }
        /*if (health_Bar != null)
        {
            health_Bar.UpdateHealthBar(PlayerHP);
        }*/
    }



    //�뽬Ÿ�� [idle, move, dash, dashTurn]
    public float dashTime;
    public bool dash;
    public bool turn;
    private void DashTime()
    {
        if (dash)
        {
            dashTime += Time.deltaTime;
        }
    }

    public int EatKirbyFormNum;
    public int KirbyFormNum;
    [PunRPC]
    public void KirbyForm() //Ŀ�� ���� ���� ���� ��ȣ�� ���� ���� ���� ����
    {
        switch (KirbyFormNum)
        {
            case 1: //���� ��
                curAbility = gameObject.AddComponent<Ability_Eat>();
                break;
            case 2: //�ִϸ� ��
                curAbility = gameObject.AddComponent<Ability_Animal>();
                break;
            case 3: //Ŀ�� ��
                curAbility = gameObject.AddComponent<Ability_Cutter>();
                break;
            case 4: //ť�ǵ� ��
                curAbility = gameObject.AddComponent<Ability_Cupid>();
                break;
            case 5: //������ ��
                curAbility = gameObject.AddComponent<Ability_Sword>();
                break;
            case 6: //�� ��
                curAbility = gameObject.AddComponent<Ability_Whell>();
                break;

            default:
                Debug.LogError("Invalid KirbyFormNum: " + KirbyFormNum);
                return;
        }
        if (curAbility == null) return; //�����Ƽ�� ������ ����
        curAbility.OnAbilityCopied(this);
    }

    [PunRPC]
    public void ChangeState()
    {
        stateMachine.ChangeState(curAbility.GetComponent<Ability_Eat>().attackState);
    }


    [PunRPC]
    public void Change_Animator_Controller(int playerViewID)
    {        
        PhotonView playerView = PhotonView.Find(playerViewID);
        if (playerView == null)
        {
            Debug.LogError("PhotonView not found for ViewID: " + playerViewID);
            return;
        }

        Animator animator = playerView.GetComponentInChildren<Animator>();
        PhotonAnimatorView animatorView = playerView.GetComponentInChildren<PhotonAnimatorView>();

        if (animatorView == null || animator == null)
        {
            Debug.LogError("Animator or PhotonAnimatorView is null.");
            return;
        }
        animatorView.enabled = false; //Synchronization�� ���� ��Ȱ��ȭ

        // Clear existing synchronization
        animatorView.GetSynchronizedParameters().Clear();
        animatorView.GetSynchronizedLayers().Clear();

        // Synchronize parameters
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Bool)
            {
                animatorView.SetParameterSynchronized(param.name, PhotonAnimatorView.ParameterType.Bool, PhotonAnimatorView.SynchronizeType.Discrete);
            }
            else if (param.type == AnimatorControllerParameterType.Float)
            {
                animatorView.SetParameterSynchronized(param.name, PhotonAnimatorView.ParameterType.Float, PhotonAnimatorView.SynchronizeType.Discrete);
            }
            else if (param.type == AnimatorControllerParameterType.Int)
            {
                animatorView.SetParameterSynchronized(param.name, PhotonAnimatorView.ParameterType.Int, PhotonAnimatorView.SynchronizeType.Discrete);
            }
            else
            {
                Debug.LogWarning($"Unsupported parameter type: {param.type} for parameter {param.name}");
            }
        }

        // Synchronize layers
        for (int i = 0; i < animator.layerCount; i++)
        {
            animatorView.SetLayerSynchronized(i, PhotonAnimatorView.SynchronizeType.Discrete);
        }
        animatorView.enabled = true; // Enable the animator view to allow synchronization
    }

    public virtual void AnimationFinishTrigger() => stateMachine.state.AnimationFinishTrigger();



    public void Call_RPC(string rpc_Name, RpcTarget type)
    {
        pView.RPC(rpc_Name, type);
    }


    [PunRPC]
    public void RPC_HitFlash()
    {
        StartCoroutine(HitFlash());
    }

    private IEnumerator HitFlash() //�ǰݽ� ���� ��ȭ
    {
        if (spriteRenderer == null)
            yield break;
/*
        for (int i = 0; i < 2; i++)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }*/
    }


    bool isInvincible = false;

    [PunRPC]
    public void RPC_StartNoDamage(float duration, float interval)
    {
        StartCoroutine(NoDamage(duration, interval));
    }

    [PunRPC]
    private IEnumerator NoDamage(float MaxTime, float interval)
    {
        isInvincible = true;

        Animator animator = GetComponentInChildren<Animator>();
        if (animator == null)
            yield break;

        SpriteRenderer spriteRenderer = animator.GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
            yield break;

        Color originalColor = spriteRenderer.color;

        float elapsedTime = 0f;
        bool isWhite = false;

        while (elapsedTime < MaxTime)
        {
            isWhite = !isWhite;
            spriteRenderer.color = isWhite ? Color.red : originalColor; // 빨간색으로 깜빡이기
            yield return new WaitForSeconds(interval);
            elapsedTime += interval;
        }

        spriteRenderer.color = originalColor;
        isInvincible = false;
    }


    public void Die_Player()
    {

        Animator animator = GetComponentInChildren<Animator>();
        if (animator != null)
        {
            SpriteRenderer spriteRenderer = animator.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                Color originalColor = spriteRenderer.color; // 기존 색상 저장
                StartCoroutine(FlickSprite(spriteRenderer, originalColor, 2f, 0.1f)); // 2초 동안 0.1초 간격으로 깜박임 <<이거 RPC화 시켜야함
            }
        }
        stateMachine.ChangeState(dieState); //죽는 애니메이션
    }

    IEnumerator FlickSprite(SpriteRenderer spriteRenderer, Color originalColor, float duration, float interval)
    {
        float elapsedTime = 0f;
        bool isVisible = true;

        while (elapsedTime < duration)
        {
            isVisible = !isVisible; // 가시성 토글
            spriteRenderer.color = isVisible ? originalColor : new Color(originalColor.r, originalColor.g, originalColor.b, 0f); // 투명도 조정
            yield return new WaitForSeconds(interval);
            elapsedTime += interval;
        }
        this.gameObject.SetActive(false);
        // 깜박임 종료 후 원래 색상 복원
        spriteRenderer.color = originalColor;
        ResurrectionPlayer();
    }

    public void ResurrectionPlayer()
    {
        if (PlayerLife > 0)
        {

            PlayerLife--;
            if (lifeNum != null)
            {
                lifeNum.UpdateLifeNum(PlayerLife);
            }

            Vector3 resPosition = Vector3.zero;
            bool foundPlayer = false;

            string areaString = this.GetComponent<PlayerTest>().area;
            foreach (GameObject player in GameManager.Instance.playerList)
            {
                if (player.activeSelf && player.GetComponent<Player>() != this && player.GetComponent<PlayerTest>().area.Equals(areaString))
                {
                    resPosition = player.transform.position + Vector3.up * 6f;
                    foundPlayer = true;
                    break;
                }
            }
            if (!foundPlayer)
            {
                SavePoint savePoint = GameManager.Instance.spwanTransform;
                if (savePoint != null)
                {
                    resPosition = savePoint.transform.position;
                    Debug.Log(savePoint.areaName + " : " + GetComponent<PlayerTest>().area + "!!!!!!!!");
                    if (savePoint.areaName != GetComponent<PlayerTest>().area)
                    {
                        //카메라 설정
                        GetComponent<PlayerTest>().area = savePoint.areaName;
                        CinemachineConfiner2D tmpCam = GameObject.Find("PlayerCamera").GetComponent<CinemachineConfiner2D>();
                        tmpCam.BoundingShape2D = savePoint.confinderArea;
                    }
                }
                else
                {
                    resPosition = GameObject.Find("SpawPoint").transform.position;
                }

            }


            Init_Player();
            this.transform.position = resPosition;
            this.gameObject.SetActive(true);
            stateMachine.ChangeState(idleState);


        }
        else
        {
            //게임오버
            Debug.Log("게임오버");
        }
        isBusy = false;
    }
    public void Init_Player()
    {

        PlayerHP = PlayerMaxHP;
        KirbyFormNum = 0;
        EatKirbyFormNum = 0;

        if (health_Bar != null)
        {
            health_Bar.UpdateHealthBar(PlayerHP);
        }

    }
}
