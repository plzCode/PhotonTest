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

    public bool TimeBool = true;

    public GameObject dashEffect;
    public Transform dashEffectPos;
    public GameObject AirJumpOutEffect;
    public Transform AirJumpOutEffectPos;
    public GameObject EatEffect1;
    public GameObject EatEffect2;
    public Transform EatEffectPos;
    public GameObject Attack;
    public Collider2D currentEnemy;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundLine;
    [SerializeField] private LayerMask whatIsGround;



    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }

    #region ����
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

    protected PhotonView pView;


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

        LastMove = 1f; //�÷��̾� ������ �Է°��� �ʱ� 1�� ����
    }

    public void Update()
    {
        stateMachine.state.Update();

        DashTime(); //�뽬��ȣ�ۿ� Ÿ��

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
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            KirbyFormNum = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            KirbyFormNum = 0;
        }

    }   

    public int EatKirbyFormNum;
    public int KirbyFormNum;
    [PunRPC]
    public void KirbyFrom() //Ŀ�� ���� ���� ���� ��ȣ�� ���� ���� ���� ����
    {
        Debug.Log("�����");
            switch (KirbyFormNum)
            {
            case 1: //���� ��
                    curAbility = gameObject.AddComponent<Ability_Eat>();
                    curAbility.OnAbilityCopied(this);
                    break;
            case 2: //�ִϸ� ��
                    curAbility = gameObject.AddComponent<Ability_Animal>();
                    curAbility.OnAbilityCopied(this);
                    break;
            }
    }


    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;


        yield return new WaitForSeconds(_seconds);

        isBusy = false;
    }


    //�浹üũ
    public bool IsGroundCheck() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundLine, whatIsGround);

    public void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundLine));
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
        rb.linearVelocity = new Vector2 (xlineVelocity, ylineVelocity);
        FlipController(xlineVelocity);
    }
    #endregion

    public void EffectAdd(float _x, GameObject Effect, Transform EffecPos) //����Ʈ�� �߰���
    {
        if (_x > 0) //�������̸� �״�� ��ȯ
        {
            //Instantiate(obj, EffecPos.position, Quaternion.identity);
            PhotonNetwork.Instantiate("Player_Effect/"+Effect.name, EffecPos.position, Quaternion.identity);

        }
        else if (_x < 0) //�����̸� �¿���� ��ȯ
        {
            //Instantiate(obj, EffecPos.position, Quaternion.Euler(0, 180, 0));
            PhotonNetwork.Instantiate("Player_Effect/" + Effect.name, EffecPos.position, Quaternion.Euler(0, 180, 0));
        }
    }



    [PunRPC]
    public void AttackAdd(float _x, string Effect, Vector3 EffecPos) //�ڽ� ��ü�� ��ȯ �� �ٸ� ��ũ��Ʈ���� ������ ���� �����ϰ�
    {
        if (_x > 0)
        {
            //Attack = Instantiate(Effect, EffecPos.position, Quaternion.identity, parentTransform);
            Attack = PhotonNetwork.Instantiate("Player_Effect/" + Effect, EffecPos, Quaternion.identity);
        }
        else if (_x < 0) //�����̸� �¿���� ��ȯ
        {
            //Attack = Instantiate(Effect, EffecPos.position, Quaternion.Euler(0, 180, 0), parentTransform);
            Attack = PhotonNetwork.Instantiate("Player_Effect/" + Effect, EffecPos, Quaternion.Euler(0, 180, 0));
        }
        Attack.GetComponent<EatEffect>().player = this;
        Attack.transform.SetParent(this.transform);
    }

    [PunRPC]
    public void AttackDestroy()
    {
        //Destroy(Attack);
        PhotonNetwork.Destroy(Attack);
        
    }

    public void SetCurrentTarget(Collider2D enemyCollider)
    {
        currentEnemy = enemyCollider;
    }

    public void TakeDamage(Vector2 EnemyAttackPos, float Damage)    //������ ���� ������ ����
    {
        if (transform.position.x > EnemyAttackPos.x)
        {
            LastMove = 1f;
        }
        else if (transform.position.x < EnemyAttackPos.x)
        {
            LastMove = -1f;
        }

        PlayerHP -= Damage;
        stateMachine.ChangeState(damageState);  //�������°� ����
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
    public void Call_RPC(string rpc_Name, RpcTarget type)
    {
        pView.RPC(rpc_Name, type);
    }

}
