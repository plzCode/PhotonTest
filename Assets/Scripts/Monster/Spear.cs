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
        target = FindClosestPlayer();

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

    private Transform FindClosestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        Transform closest = null;
        float minDistance = Mathf.Infinity;
        Vector3 currentPos = transform.position;

        foreach (GameObject player in players)
        {
            float dist = Vector3.Distance(player.transform.position, currentPos);
            if (dist < minDistance)
            {
                minDistance = dist;
                closest = player.transform;
            }
        }

        return closest;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("�÷��̾�� " + power + "��ŭ �������� �ݴϴ�.");
            collision.GetComponent<Player>().TakeDamage(transform.position, power);
            Destroy(gameObject);
        }
    }
}
