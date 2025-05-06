using Photon.Pun;
using UnityEngine;

public class KirbyDamageStar : Item
{
    public Player player;
    public EnemyNumber enemyNumber;

    public float SaveNumber;

    public float moveSpeed = 3f;
    public float randomSpeed = 0f;

    public float dleeteTime;

    private void Awake()
    {
        /*player = GameObject.FindWithTag("Player").GetComponent<Player>();*/
        enemyNumber = GetComponent<EnemyNumber>();  // 자기 자신에 붙어 있는 EnemyNumber 가져오기
    }

    void Start()
    {
        //enemyNumber.Number = player.EatKirbyFormNum;
        /*player.KirbyFormNum = 0;
        player.EatKirbyFormNum = 0;*/
    }

    void Update()
    {
        if (this.photonView.IsMine)
        {
            transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
        }

        dleeteTime += Time.deltaTime;

        if (dleeteTime > 8f && this.photonView.IsMine)
        {
            DestroySelf();
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (!this.photonView.IsMine) return;

        if (collision.gameObject.CompareTag("Ground"))
        {
            randomSpeed = Random.Range(-3f, 3f);
            moveSpeed = randomSpeed;
        }
    }

    [PunRPC]
    public void SetPlayer(int pView)
    {
        player = PhotonView.Find(pView).GetComponent<Player>();
        /*if(player.curAbility is Ability_Eat)
        {
            enemyNumber.Number = player.EatKirbyFormNum;
        }
        else if (player.curAbility != null)
        {
            enemyNumber.Number = player.KirbyFormNum;
        }*/
        enemyNumber.Number = player.EatKirbyFormNum;


    }
}
