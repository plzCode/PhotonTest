using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
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

    //���� �����Ƽ
    public Ability_Eat EatKirby => GetComponentInParent<Ability_Eat>();

    //For Test Ability
    public PlayerAbility curAbility;
    public bool isInhaling = false;
    [SerializeField]
    public Transform attackCheck;

    public PhotonView pView;


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
        //Debug.Log(KirbyFormNum);
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
        Debug.Log(EatKirbyFormNum);
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

        //�÷��̾� idle ���·� �ű�
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (curAbility != null)
            {
                curAbility.AttackHandle();
            }
        }

        #endregion
        Hill();

    }   

    public int EatKirbyFormNum;
    public int KirbyFormNum;
    [PunRPC]
    public void KirbyForm() //Ŀ�� ���� ���� ���� ��ȣ�� ���� ���� ���� ����
    {
        Debug.Log("KirbyFrom �����");
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


    //�浹üũ
    public bool IsGroundCheck() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundLine, whatIsGround | whatIsHill);
    public bool IsHillCheck() => Physics2D.Raycast(hillCheck.position, Vector2.down, hillLine, whatIsHill);

    public void Hill() //������� ĳ���Ͱ� �̲������� �ʰ� ��
    {
        RaycastHit2D hit = Physics2D.Raycast(hillCheck.position, Vector2.down, groundLine, whatIsHill);

        if(hit)
        {
            perp = Vector2.Perpendicular(hit.normal).normalized;
            angle = Vector2.Angle(hit.normal, Vector2.up);

            if (angle != 0)
                isSlope = true;
            else
                isSlope = false;

            Debug.DrawLine(hit.point, hit.point + hit.normal, Color.blue);
            Debug.DrawLine(hit.point, hit.point + perp, Color.red);
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawLine(hillCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundLine));
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
    public void AttackAdd(float _x, string effectName, Vector3 effectPos)
    {
        GameObject effect = null;

        if (_x > 0)
            effect = PhotonNetwork.Instantiate("Player_Effect/" + effectName, effectPos, Quaternion.identity);
        else if (_x < 0)
            effect = PhotonNetwork.Instantiate("Player_Effect/" + effectName, effectPos, Quaternion.Euler(0, 180, 0));

        if (effect != null)
        {
            effect.GetComponent<EatEffect>().player = this;
            effect.transform.SetParent(this.transform);
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
    public void PerformAttack() //�÷��̾� �������� ���� ����
    {
        switch (KirbyFormNum)
        {
            case 1:
                EatKirby.Attack();
                break;
            case 2:
                curAbility.AttackHandle();
                break;
        }
    }


    public void SetCurrentTarget(Collider2D enemyCollider)
    {
        currentEnemy = enemyCollider;
    }

    [PunRPC]
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

        if(KirbyFormNum > 0)
        {
            EffectAdd(LastMove, DamageStar, transform);
            curAbility.OnAbilityDestroyed(this); //�����Ƽ �ʱ�ȭ
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

    /*[PunRPC]
    public void Change_Animator_Controller(int playerID)
    {
        PhotonView playerView = PhotonView.Find(playerID);
        RuntimeAnimatorController animatorContoller = playerView.GetComponentInChildren<Animator>().runtimeAnimatorController;
        for (int i = 0; i < animatorContoller.animationClips.Length; i++)
        {
            playerView.GetComponentInChildren<PhotonAnimatorView>().SetParameterSynchronized(animatorContoller.animationClips[i].name, PhotonAnimatorView.ParameterType.Bool, PhotonAnimatorView.SynchronizeType.Discrete);
        }
    }*/

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
        RuntimeAnimatorController newController = animator.GetComponent<Animator>().runtimeAnimatorController;
        PhotonAnimatorView animatorView = playerView.GetComponentInChildren<PhotonAnimatorView>();

        // Animator Controller ����
        animator.runtimeAnimatorController = newController;

        // PhotonAnimatorView�� ����ȭ �Ķ���� �ʱ�ȭ
        if (animatorView != null)
        {
            animatorView.GetSynchronizedParameters().Clear();
            animatorView.GetSynchronizedLayers().Clear();
            animatorView.SetLayerSynchronized(0, PhotonAnimatorView.SynchronizeType.Discrete);
            // �� Animator Controller�� �Ķ���͸� ����ȭ ����
            foreach (AnimatorControllerParameter param in animator.parameters)
            {
                if (param.type == AnimatorControllerParameterType.Bool)
                {
                    animatorView.SetParameterSynchronized(param.name, PhotonAnimatorView.ParameterType.Bool, PhotonAnimatorView.SynchronizeType.Discrete);
                }
                else if (param.type == AnimatorControllerParameterType.Float)
                {
                    animatorView.SetParameterSynchronized(param.name, PhotonAnimatorView.ParameterType.Float, PhotonAnimatorView.SynchronizeType.Continuous);
                }
                else if (param.type == AnimatorControllerParameterType.Int)
                {
                    animatorView.SetParameterSynchronized(param.name, PhotonAnimatorView.ParameterType.Int, PhotonAnimatorView.SynchronizeType.Discrete);
                }
                else if (param.type == AnimatorControllerParameterType.Trigger)
                {
                    animatorView.SetParameterSynchronized(param.name, PhotonAnimatorView.ParameterType.Trigger, PhotonAnimatorView.SynchronizeType.Discrete);
                }
            }
        }
    }

    public virtual void AnimationFinishTrigger() => stateMachine.state.AnimationFinishTrigger();


    public void Call_RPC(string rpc_Name, RpcTarget type)
    {
        pView.RPC(rpc_Name, type);
    }
    

}
