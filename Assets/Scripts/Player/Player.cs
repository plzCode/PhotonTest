using Photon.Realtime;
using System;
using System.Collections;
using System.Resources;
using UnityEngine;
using UnityEngine.InputSystem.XInput;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public float MoveSpeed;
    public float DashSpeed;

    public float dashTime;
    public bool dash = false;
    public bool turn;

    public float MinJumpPower;
    public float JumpPower;
    public float MaxJumpPower = 3f;

    public bool isBusy { get; private set; }

    public bool flipbool = true;

    [SerializeField] private GameObject dashEffect;
    [SerializeField] private Transform dashEffectPos;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundLine;
    [SerializeField] private LayerMask whatIsGround;



    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }



    public PlayerStateMachine stateMachine { get; private set; }

    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerDashTurn dashTurnState { get; private set; }
    public PlayerDownState downState { get; private set; }

    public PlayerJumpState jumpState { get; private set; }
    public PlayerDowningState downingState { get; private set; }
    public PlayerDowningGroundState downingGroundState { get; private set; }

    public PlayerAirState airState { get; private set; }
    public PlayerAirJump airJumpState { get; private set; }
    public PlayerAirJumping airJumpingState { get; private set; }
    public PlayerAirJumpUp airJumpUpState { get; private set; }
    



    public void Awake()
    {
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        dashTurnState = new PlayerDashTurn(this, stateMachine, "DashTurn");
        downState = new PlayerDownState(this, stateMachine, "Down");

        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        downingState = new PlayerDowningState(this, stateMachine, "Downing");
        downingGroundState = new PlayerDowningGroundState(this, stateMachine, "DowningGround");

        airState = new PlayerAirState(this, stateMachine, "Jump");
        airJumpState = new PlayerAirJump(this, stateMachine, "AirJump");
        airJumpingState = new PlayerAirJumping(this, stateMachine, "AirJumping");
        airJumpUpState = new PlayerAirJumpUp(this, stateMachine, "AirJumpUp");
    }

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();

        stateMachine.StartState(idleState);
    }

    public void Update()
    {
        stateMachine.state.Update();

        if (dash)
        {
            dashTime += Time.deltaTime;
        }
    }






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


    //이미지 전환
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
        }
        else if (_x < 0 && flipbool)
        {
            Flip();
        }
    }

    //캐릭터 움직임
    public void lineVelocity(float xlineVelocity, float ylineVelocity)
    {
        rb.linearVelocity = new Vector2 (xlineVelocity, ylineVelocity);
        FlipController(xlineVelocity);
    }

    public void ZerolineVelocity(float xlineVelocity, float ylineVelocity)
    {
        rb.linearVelocity = new Vector2(xlineVelocity, ylineVelocity);
    }

    public void DashEffect(float _x)
    {
        if (_x > 0)
        {
            Instantiate(dashEffect, dashEffectPos.position, Quaternion.identity);
        }
        else if (_x < 0)
        {
            Instantiate(dashEffect, dashEffectPos.position, Quaternion.Euler(0, 180, 0));
        }
    }

}
