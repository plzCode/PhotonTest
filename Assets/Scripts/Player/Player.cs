using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Resources;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XInput;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public float PlayerHP;

    public float MoveSpeed;
    public float DashSpeed;

    public float JumpPower;

    public bool isBusy { get; private set; }

    public bool flipbool = true;
    public float LastMove;

    public GameObject dashEffect;
    public Transform dashEffectPos;
    public GameObject AirJumpOutEffect;
    public Transform AirJumpOutEffectPos;
    public GameObject EatEffect;
    public Transform EatEffectPos;
    public GameObject Attack;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundLine;
    [SerializeField] private LayerMask whatIsGround;



    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }

    #region 스탯
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

    public PlayerEating12State eatingState { get; private set; }
    public PlayerEatingEndState eatingEndState { get; private set; }

    public PlayerDamageState damageState { get; private set; }

    #endregion

    //For Test Ability
    private PlayerAbility curAbility;
    public bool isInhaling = false;


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

        eatingState = new PlayerEating12State(this, stateMachine, "Eating1");
        eatingEndState = new PlayerEatingEndState(this, stateMachine, "EatingEnd");

        damageState = new PlayerDamageState(this, stateMachine, "Damage");
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
        stateMachine.state.Update();

        DashTime(); //대쉬상호작용 타임

        #region TestRegion
        if (Input.GetKeyDown(KeyCode.Z) && GetComponent<PhotonView>().IsMine)
        {
            if (curAbility != null)
            {
                curAbility.AttackHandle();
            }
            else
            {
                Debug.Log("Normal Kirby Attack");
            }
        }

        if (Input.GetKeyDown(KeyCode.X) && GetComponent<PhotonView>().IsMine)
        {
            if (curAbility == null)
            {
                curAbility = gameObject.AddComponent<Ability_Animal>();
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

    #region TestRegion2
    /*IEnumerator StartInhale()
    {
        while()
    }*/
    #endregion




    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;


        yield return new WaitForSeconds(_seconds);

        isBusy = false;
    }


    //충돌체크
    public bool IsGroundCheck() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundLine, whatIsGround);

    public void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundLine));
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
        rb.linearVelocity = new Vector2 (xlineVelocity, ylineVelocity);
        FlipController(xlineVelocity);
    }
    #endregion

    public void EffectAdd(float _x, GameObject Effect, Transform EffecPos) //이펙트를 추가함
    {
        if (_x > 0) //오른쪽이면 그대로 소환
        {
            //Instantiate(obj, EffecPos.position, Quaternion.identity);
            PhotonNetwork.Instantiate("Player_Effect/"+Effect.name, EffecPos.position, Quaternion.identity);

        }
        else if (_x < 0) //왼쪽이면 좌우반전 소환
        {
            //Instantiate(obj, EffecPos.position, Quaternion.Euler(0, 180, 0));
            PhotonNetwork.Instantiate("Player_Effect/" + Effect.name, EffecPos.position, Quaternion.Euler(0, 180, 0));
        }
    }

    public void AttackAdd(float _x, GameObject Effect, Transform EffecPos, Transform parentTransform) //자식 객체로 소환 및 다른 스크립트에서 프리펩 삭제 가능하게
    {
        if (_x > 0)
        {
            Attack = Instantiate(Effect, EffecPos.position, Quaternion.identity, parentTransform);
        }
        else if (_x < 0) //왼쪽이면 좌우반전 소환
        {
            Attack = Instantiate(Effect, EffecPos.position, Quaternion.Euler(0, 180, 0), parentTransform);
        }
    }

    public void AttackDestroy()
    {
        Destroy(Attack);
    }

    public float EnemyAttackLastPos;
    public void TakeDamage(Vector2 EnemyAttackPos, float Damage)    //몬스터의 공격 데미지 실행
    {
        if (transform.position.x > EnemyAttackPos.x)
        {
            EnemyAttackLastPos = 1f;
        }
        else if (transform.position.x < EnemyAttackPos.x)
        {
            EnemyAttackLastPos = -1f;
        }
        else
        {
            EnemyAttackLastPos = flipbool ? -1f : 1f;
        }

        PlayerHP -= Damage;
        stateMachine.ChangeState(damageState);  //굴러가는거 실행
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
}
