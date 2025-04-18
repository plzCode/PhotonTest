using UnityEngine;

public class Spear : MonsterWeapon
{

    private Rigidbody2D rb;
    private Transform target;
    //private bool launched = false;

    [SerializeField] private float flightTime = 1.0f; // ��â�� ��ǥ ������ ������ �ð�

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

        // ���� �ӵ� ���
        float vx = distance.x / flightTime;

        // ���� �ӵ� ���: y = vy * t + 0.5 * g * t^2 �� vy = (y - 0.5 * g * t^2) / t
        float gravity = Mathf.Abs(Physics2D.gravity.y * rb.gravityScale);
        float vy = (distance.y + 0.5f * gravity * Mathf.Pow(flightTime, 2)) / flightTime;

        // ���� �ʱ� �ӵ� ����
        Vector2 velocity = new Vector2(vx, vy);
        rb.linearVelocity = velocity;

        //launched = true;

        // ȸ�� ���� ���� (���û���)
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("�÷��̾�� " + power + "��ŭ �������� �ݴϴ�.");
            //player.TakeDamage(power);
            Destroy(gameObject);
        }
    }
}
