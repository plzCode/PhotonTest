using Photon.Pun;
using Unity.Services.Authentication.PlayerAccounts;
using UnityEngine;

public class EatEffect : MonoBehaviour
{
    public Player player;
    public Transform enemy;
    public PhotonView pView;
    public bool Eat;
    public int PormNumber;

    void Start()
    {
        
    }

    void Update()
    {
    }
    [PunRPC]
    public void EatEnemy()
    {
        if (enemy == null) return; //이미 먹고있다면 리턴
        if (Vector2.Distance(player.transform.position, enemy.position) < 1f) //적과의 거리가 1보다 작다면
        {
            PhotonNetwork.Destroy(enemy.gameObject);  //적삭제
            player.stateMachine.ChangeState(player.eatState); //플레이어 먹은모션
            player.EatKirbyFormNum = PormNumber; //먹는 커비 모션에 적의 변신 번호 값을 저장
            player.KirbyFormNum = 1; //먹는 커비로 변신
            //PlayerEetState에서 변신 함수 씀 
        }
    }

    public void OnTriggerStay2D(Collider2D collision) //적과 충돌중 이라면
    {
        //if (!pView.IsMine) return;
        if(collision == null) return; //충돌중인 적이 없다면 리턴
        if (collision.gameObject.CompareTag("Enemy")) //충돌중인 적 콜라이더를 가져옴
        {
            Collider2D playerCol = player.GetComponent<Collider2D>(); //플레이어 콜라이더를 가져옴
            Collider2D enemyCol = collision.GetComponent<Collider2D>(); //충돌중인 적 콜라이더를 가져옴
            PormNumber = collision.GetComponent<EnemyNumber>().Number; //흡입하는 적의 변신 번호를 가져옴니다. (적에게 EnemyNumber 스크립트가 있어야하고 번호도 있어야 합니다)

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