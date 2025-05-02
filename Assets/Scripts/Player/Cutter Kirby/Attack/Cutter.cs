using Photon.Pun;
using UnityEngine;

public class Cutter : PlayerRagedManager
{
    private Rigidbody2D rb;
    private Transform target;
    private Animator anim;

    [SerializeField] private float flightTime = 1.0f;

    public float speed = 10f;
    private float lifeTime = 10f;  // 초기 수명 설정

    public bool AttackDelay = false;
    public bool AttackEnemy = false;
    public bool ReturnPlayer = false;

    // Rigidbody2D와 PhotonView를 초기화합니다.
    public void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void Start()
    {
        if (!photonView.IsMine) return; // 현재 클라이언트가 소유하지 않은 객체는 초기화하지 않음

        if (player.CutterUpgrade == 0)
        {
            anim.SetBool("Basic", true);  // 업그레이드 상태일 때 애니메이션을 설정합니다.
        }
        else
        {
            player.curAbility.attackPower += 10; // 업그레이드시 공격력을 증가시킵니다.
            anim.SetBool("Upgrade", true); // 업그레이드 상태가 아닐 때 애니메이션을 설정합니다.
        }
    }

    // 매 프레임마다 Cutter의 이동 및 상태를 업데이트합니다.
    public void Update()
    {
        if (!photonView.IsMine) return; // 현재 클라이언트가 소유하지 않은 객체는 업데이트하지 않음


        // Cutter가 살아있는 동안
        if (lifeTime > 0)
        {
            lifeTime -= Time.deltaTime;  // 시간이 지나면서 수명이 감소합니다.

            if (lifeTime <= 0f)
            {
                // 생명 주기가 끝나면 Cutter를 제거합니다.
                PhotonNetwork.Destroy(gameObject);
            }
        }

        if (!AttackEnemy)
        {
            if (Input.GetKey(KeyCode.Mouse0) && !AttackEnemy)
            {
                if (Input.GetKey(KeyCode.W))
                {
                    // W 키를 누르면 위로 이동합니다.
                    transform.Translate(0, 1f * Time.deltaTime, 0);
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    // S 키를 누르면 아래로 이동합니다.
                    transform.Translate(0, -1f * Time.deltaTime, 0);
                }
            }
            transform.Translate(speed * Time.deltaTime, 0, 0);
            // 속도를 점진적으로 감소시킵니다.
            speed -= 10 * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!photonView.IsMine) return; // 현재 클라이언트가 소유하지 않은 객체는 충돌 처리하지 않음

        if (collision.gameObject.CompareTag("Enemy"))
        {
            lifeTime = 10f;  // 적과 충돌 시 생명 주기를 초기화합니다.

            // 적과 충돌 시 포물선 운동을 시작합니다.
            AttackEnemy = true;

            // 가장 가까운 플레이어의 위치를 저장합니다.
            target = FindClosestPlayer();
            if (target != null)
            {
                LaunchParabolic();
            }

            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.GetComponent<Enemy>().photonView.RPC("TakeDamage", RpcTarget.All, player.curAbility.attackPower);
            }

            else
            {
                Debug.LogWarning("Player not found!");
            }
        }

        if (collision.gameObject.CompareTag("Ground") && PhotonNetwork.IsMasterClient)
        {
            if (lifeTime <= 9.8f)
            {
                player.CutterUpgrade = 0;
            PhotonNetwork.Destroy(gameObject);  // 제거
            }
        }

        if (collision.gameObject.CompareTag("StarBlock") && PhotonNetwork.IsMasterClient)
        {
            if (lifeTime <= 9.8f)
            {
                player.CutterUpgrade = 0;
                PhotonNetwork.Destroy(gameObject);  // 제거
            }
        }


        if (collision.gameObject.CompareTag("Player") && PhotonNetwork.IsMasterClient)
        {
            if (lifeTime <= 9.8f)
            {
                PhotonNetwork.Destroy(gameObject);

                if (!AttackEnemy)
                {
                    player.CutterUpgrade = 0;
                }
                else
                    player.CutterUpgrade += 1; // 업그레이드 수치를 증가시킵니다.
            }
        }
    }

    // 포물선 운동을 계산하여 Cutter의 속도를 설정합니다.
    private void LaunchParabolic()
    {
        Vector2 start = transform.position; // 시작 위치
        Vector2 end = target.position; // 목표 위치

        Vector2 distance = end - start; // 시작 위치와 목표 위치 간의 거리

        // x축 속도를 계산합니다. 목표까지의 x축 거리를 비행 시간으로 나눕니다.
        float vx = distance.x / flightTime;

        rb.gravityScale = 1f;
        // 중력 값을 계산합니다. Rigidbody2D의 중력 스케일을 고려합니다.
        float gravity = Mathf.Abs(Physics2D.gravity.y * rb.gravityScale);

        // y축 속도를 계산합니다. 중력과 비행 시간을 고려하여 계산합니다.
        float vy = (distance.y + 0.5f * gravity * Mathf.Pow(flightTime, 2)) / flightTime;

        // 계산된 x축과 y축 속도를 사용하여 최종 속도를 설정합니다.
        Vector2 velocity = new Vector2(vx, vy);
        rb.linearVelocity = velocity;

        // 이동 방향에 따라 회전 각도를 설정합니다.
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    // 가장 가까운 플레이어를 찾습니다.
    private Transform FindClosestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player"); // 모든 플레이어를 찾습니다.
        Transform closest = null;
        float minDistance = Mathf.Infinity; // 초기 최소 거리를 무한대로 설정합니다.
        Vector3 currentPos = transform.position; // 현재 위치

        foreach (GameObject player in players)
        {
            // Player 컴포넌트를 가져옵니다.
            Player playerComponent = player.GetComponent<Player>();
            if (playerComponent == null || playerComponent.KirbyFormNum != 3)
            {
                // Player 컴포넌트가 없거나 KirbyFormNum이 3이 아니면 건너뜁니다.
                continue;
            }

            // 각 플레이어와의 거리를 계산합니다.
            float dist = Vector3.Distance(player.transform.position, currentPos);
            if (dist < minDistance)
            {
                // 더 가까운 플레이어를 찾으면 최소 거리와 가장 가까운 플레이어를 업데이트합니다.
                minDistance = dist;
                closest = player.transform;
            }
        }

        return closest; // KirbyFormNum이 3인 가장 가까운 플레이어의 Transform을 반환합니다.
    }
}