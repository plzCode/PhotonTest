using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float PlayerHP;
    public float PlayerMaxHP = 100f; //플레이어 최대 체력

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



    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }

    #region 스탯
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
    //Inventory
    public Inventory inventory;


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

        changeFormState = new PlayerChageFormState(this, stateMachine, "ChangeForm");

        pView = GetComponent<PhotonView>();
    }

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        stateMachine.StartState(idleState);

        LastMove = 1f; //플레이어 마지막 입력값을 초기 1로 만듬
    }

    public void Update()
    {
        //Debug.Log(KirbyFormNum);
        stateMachine.state.Update();

        DashTime(); //대쉬상호작용 타임
        Hill();

        #region TestRegion
        if (pView.IsMine == false) return; //내 캐릭터가 아닐때는 리턴        

        if (Input.GetKeyDown(KeyCode.X))
        {
            if (curAbility == null)
            {
                curAbility = gameObject.AddComponent<Ability_Eat>();
                curAbility.OnAbilityCopied(this);
            }
            else
            {
                curAbility.OnAbilityDestroyed(this);
                Destroy(curAbility);
            }

        }
        #endregion
    }




    [PunRPC]
    public void SyncFormNum()
    {
        KirbyFormNum = EatKirbyFormNum;
    }


    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;


        yield return new WaitForSeconds(_seconds);

        isBusy = false;
    }


    //충돌체크
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
            isSlope = false; // 언덕이 아닌 경우
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundLine);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(hillCheck.position, new Vector3(hillCheck.position.x, hillCheck.position.y - hillLine));
    }

    #region 캐릭터 움직임 및 좌우반전
    //이미지 좌우반전
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

    //캐릭터 움직임
    [PunRPC]
    public void lineVelocity(float xlineVelocity, float ylineVelocity)
    {
        if (rb == null) return;

        if (isSlope && IsHillCheck())
        {
            // 경사면이면, 경사 방향으로 이동
            rb.linearVelocity = perp * xlineVelocity * -1f;
            rb.linearVelocity = new Vector2(xlineVelocity, ylineVelocity);
        }
        else
        {
            // 평지면 그냥 일반 x축 이동
            rb.linearVelocity = new Vector2(xlineVelocity, ylineVelocity);
        }

        FlipController(xlineVelocity);
    }
    #endregion

    [PunRPC]
    public void EffectAdd(float _x, GameObject Effect, Transform EffecPos) //이펙트를 추가함
    {
        GameObject bullet = null;

        if (_x > 0) //오른쪽이면 그대로 소환
        {
            //Instantiate(obj, EffecPos.position, Quaternion.identity);
            bullet = PhotonNetwork.Instantiate("Player_Effect/" + Effect.name, EffecPos.position, Quaternion.identity);
            

        }
        else if (_x < 0) //왼쪽이면 좌우반전 소환
        {
            //Instantiate(obj, EffecPos.position, Quaternion.Euler(0, 180, 0));
            bullet = PhotonNetwork.Instantiate("Player_Effect/" + Effect.name, EffecPos.position, Quaternion.Euler(0, 180, 0));
        }

        if (bullet != null)
        {
            PlayerRagedManager attackScript = bullet.GetComponent<PlayerRagedManager>();
            if (attackScript != null)
            {
                attackScript.player = this; // 이 코드가 Player 클래스 안에 있어야 함
            }

            /*if(bullet.GetComponent<KirbyDamageStar>() != null)
            {
                bullet.GetComponent<KirbyDamageStar>().player = this; // 이 코드가 Player 클래스 안에 있어야 함
                bullet.GetComponent<KirbyDamageStar>().enemyNumber.Number = this.EatKirbyFormNum; //적의 번호를 가져옴
            }*/
        }
    }



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
            if(effect.GetComponent<EatEffect>() != null)
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

        AttackList.Clear(); // 리스트 초기화
    }

    [PunRPC]
    public void EatEnemy(int enemyViewId)
    {
        Debug.Log("EatEnemy");
        //if (enemy == null || this == null) return; //이미 먹고있다면 리턴
        PhotonView eView = PhotonView.Find(enemyViewId);
        Debug.Log(enemyViewId);
        if (eView == null) return;
        Transform enemy = eView.transform;

        enemy.GetComponentInChildren<PhotonAnimatorView>().enabled = false; //애니메이터 비활성화
        enemy.GetComponent<PhotonView>().enabled = false; //포톤뷰 비활성화
                                                          //PhotonView eView = enemy.GetComponent<PhotonView>();
                                                          //PhotonNetwork.Destroy(enemy.gameObject);  //적삭제
                                                          //this.EatKirbyFormNum = PormNumber; //먹는 커비 모션에 적의 변신 번호 값을 저장

        Debug.Log(enemy.name + " : " + eView.ViewID);
        this.EatKirbyFormNum = enemy.GetComponent<EnemyNumber>().Number;
        this.KirbyFormNum = 1; //먹는 커비로 변신
        Debug.Log(this.GetComponent<PhotonView>().ViewID + "의 적을 먹음"); //적을 먹음
        this.stateMachine.ChangeState(this.eatState); //플레이어 먹은모션            
                                                      //PlayerEetState에서 변신 함수 씀

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
    public void TakeDamage(Vector2 EnemyAttackPos, float Damage)    //몬스터의 공격 데미지 실행
    {
        if (transform.position.x > EnemyAttackPos.x)
        {
            LastMove = 1f;
        }
        else if (transform.position.x < EnemyAttackPos.x)
        {
            LastMove = -1f;
        }

        if (KirbyFormNum > 0 && pView.IsMine) 
        {
            EffectAdd(LastMove, DamageStar, transform);
            curAbility.OnAbilityDestroyed(this); //어빌리티 초기화
        }
        PlayerHP -= Damage;
        if (health_Bar != null)
        {
            health_Bar.UpdateHealthBar(PlayerHP);
        }
        stateMachine.ChangeState(damageState);  //굴러가는거 실행
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
        if (health_Bar != null)
        {
            health_Bar.UpdateHealthBar(PlayerHP);
        }
    }



    //대쉬타임 [idle, move, dash, dashTurn]
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
    public void KirbyForm() //커비가 먹은 적의 고유 번호에 따라 변신 폼을 정함
    {
        Debug.Log("KirbyFrom 실행됨");

        switch (KirbyFormNum)
        {
            case 1: //먹은 폼
                curAbility = gameObject.AddComponent<Ability_Eat>();
                Debug.Log("먹은 폼");
                break;
            case 2: //애니멀 폼
                curAbility = gameObject.AddComponent<Ability_Animal>();
                break;
            case 3: //커터 폼
                curAbility = gameObject.AddComponent<Ability_Cutter>();
                break;
            case 4: //큐피드 폼
                curAbility = gameObject.AddComponent<Ability_Cupid>();
                break;
            case 5: //스워드 폼
                curAbility = gameObject.AddComponent<Ability_Sword>();
                break;
            case 6: //휠 폼
                curAbility = gameObject.AddComponent<Ability_Sword>();
                break;

            default:
                Debug.LogError("Invalid KirbyFormNum: " + KirbyFormNum);
                return;
        }
        if (curAbility == null) return; //어빌리티가 없으면 리턴
        curAbility.OnAbilityCopied(this);
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
        animatorView.enabled = false; //Synchronization을 위해 비활성화

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


}
