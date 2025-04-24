using Photon.Pun;
using Unity.Services.Authentication.PlayerAccounts;
using UnityEngine;

public class EatEffect : MonoBehaviour
{
    public Player player;
    public Transform enemy;
    public PhotonView pView;
    public bool Eat;

    void Start()
    {
        
    }

    void Update()
    {
    }
    [PunRPC]
    public void EatEnemy()
    {
        if (Vector2.Distance(player.transform.position, enemy.position) < 1f) //적과의 거리가 1보다 작다면
        {
            Destroy(enemy.gameObject);  //적삭제
            player.stateMachine.ChangeState(player.eatState); //플레이어 먹은모션
        }
    }

    public void OnTriggerStay2D(Collider2D collision) //적과 충돌중 이라면
    {
        //if (!pView.IsMine) return;

        if (collision.gameObject.CompareTag("Enemy")) //충돌중인 적 콜라이더를 가져옴
        {
            Collider2D playerCol = player.GetComponent<Collider2D>(); //플레이어 콜라이더를 가져옴
            Collider2D enemyCol = collision.GetComponent<Collider2D>(); //충돌중인 적 콜라이더를 가져옴

            enemy = collision.gameObject.transform;

            // 적을 플레이어 위치로 당긴다
            enemy.position = Vector2.MoveTowards(enemy.position, player.transform.position, 5f * Time.deltaTime);
            GetComponent<PhotonView>().RPC("EatEnemy", RpcTarget.All);
            //EatEnemy();
            

            if (playerCol != null && enemyCol != null) //플레이어와 충돌중인 적 콜라이더가 비어있지 않다면
                Physics2D.IgnoreCollision(playerCol, enemyCol, false); //둘이 닿을땐 서로 부딪히지 않음
        }
    }

}