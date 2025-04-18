using UnityEngine;

public class Spear : MonsterWeapon
{

    private Rigidbody2D rb;
    private Transform target;
    //private bool launched = false;

    [SerializeField] private float flightTime = 1.0f; // 투창이 목표 지점에 도달할 시간

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (target != null)
        {
            LaunchParabolic();
        }
        else
        {
            Debug.LogWarning("Player not found!");
        }
    }

    private void Update()
    {
        transform.right = rb.linearVelocity;
    }

    private void LaunchParabolic()
    {
        Vector2 start = transform.position;
        Vector2 end = target.position;
        Vector2 distance = end - start;

        // 수평 속도 계산
        float vx = distance.x / flightTime;

        // 수직 속도 계산: y = vy * t + 0.5 * g * t^2 → vy = (y - 0.5 * g * t^2) / t
        float gravity = Mathf.Abs(Physics2D.gravity.y * rb.gravityScale);
        float vy = (distance.y + 0.5f * gravity * Mathf.Pow(flightTime, 2)) / flightTime;

        // 최종 초기 속도 설정
        Vector2 velocity = new Vector2(vx, vy);
        rb.linearVelocity = velocity;

        //launched = true;

        // 회전 방향 설정 (선택사항)
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("플레이어에게 " + power + "만큼 데미지를 줍니다.");
            //player.TakeDamage(power);
            Destroy(gameObject);
        }
    }
}
