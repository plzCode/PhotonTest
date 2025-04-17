using UnityEngine;

public class Player : MonoBehaviour
{
    public float MoveSpeed;
    public float JumpPower;

    public float flipdir = 1;
    public bool flipbool = true;

    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }



    public PlayerStateMachine stateMachine { get; private set; }

    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }



    public void Awake()
    {
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
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
    }

    public void Flip()
    {
        flipdir = flipdir * -1;
        flipbool = !flipbool;
        transform.Rotate(0, 180, 0);
    }

    public void FlipController(float _x)
    {
        if (_x > 0 && !flipbool)
            Flip();
        else if (_x < 0 && flipbool)
            Flip();
    }

    public void lineVelocity(float xlineVelocity, float ylineVelocity)
    {
        rb.linearVelocity = new Vector2 (xlineVelocity, ylineVelocity);
        FlipController(xlineVelocity);
    }
}
