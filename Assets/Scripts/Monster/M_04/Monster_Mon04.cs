using UnityEngine;

public class Monster_Mon04 : Enemy
{
   [Header("플레이어 탐지")]
   public float playerDetectDistance = 5f;     

   [Header("투사체 · 스폰 위치")]
   public MonsterWeapon projectilePrefab;     
   public Transform projectileSpawn;

   [HideInInspector] public M04_IdleState idleState;
   [HideInInspector] public M04_WalkState walkState;
   [HideInInspector] public M04_Attack1State attack1State;    
   [HideInInspector] public M04_Attack2State attack2State;
   [HideInInspector] public M04_DieState dieState;

    protected override void Awake()
    {
        base.Awake();

        idleState = new M04_IdleState(this, stateMachine, "Idle");
        walkState = new M04_WalkState(this, stateMachine, "Walk");
        attack1State = new M04_Attack1State(this, stateMachine, "Attack1");
        attack2State = new M04_Attack2State(this, stateMachine, "Attack2");
        dieState = new M04_DieState(this, stateMachine, "Die");
    }



    protected override void Start()
    {
        setStart();
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
    }

    public void SpawnProjectile()
    {
        if (projectilePrefab == null || projectileSpawn == null) return;

        var proj = Instantiate(projectilePrefab, projectileSpawn.position, Quaternion.identity);
        if (facingDir < 0)
            proj.transform.Rotate(0, 180, 0);

        var rb2d = proj.GetComponent<Rigidbody2D>();
        if (rb2d != null)
            rb2d.linearVelocity = new Vector2(proj.speed * facingDir, 0);
    }

    public void ExtendAttackRange()
    {
        attackCheckRadius *= 1.5f;  

        Invoke("ResetAttackRange", 0.2f);  
    }

    private void ResetAttackRange()
    {
        attackCheckRadius /= 1.5f;  
    }





}
