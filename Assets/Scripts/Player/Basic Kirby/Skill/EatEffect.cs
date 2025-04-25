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
        if (enemy == null) return; //�̹� �԰��ִٸ� ����
        if (Vector2.Distance(player.transform.position, enemy.position) < 1f) //������ �Ÿ��� 1���� �۴ٸ�
        {
            PhotonNetwork.Destroy(enemy.gameObject);  //������
            player.stateMachine.ChangeState(player.eatState); //�÷��̾� �������
            player.EatKirbyFormNum = PormNumber; //�Դ� Ŀ�� ��ǿ� ���� ���� ��ȣ ���� ����
            player.KirbyFormNum = 1; //�Դ� Ŀ��� ����
            //PlayerEetState���� ���� �Լ� �� 
        }
    }

    public void OnTriggerStay2D(Collider2D collision) //���� �浹�� �̶��
    {
        //if (!pView.IsMine) return;
        if(collision == null) return; //�浹���� ���� ���ٸ� ����
        if (collision.gameObject.CompareTag("Enemy")) //�浹���� �� �ݶ��̴��� ������
        {
            Collider2D playerCol = player.GetComponent<Collider2D>(); //�÷��̾� �ݶ��̴��� ������
            Collider2D enemyCol = collision.GetComponent<Collider2D>(); //�浹���� �� �ݶ��̴��� ������
            PormNumber = collision.GetComponent<EnemyNumber>().Number; //�����ϴ� ���� ���� ��ȣ�� �����ȴϴ�. (������ EnemyNumber ��ũ��Ʈ�� �־���ϰ� ��ȣ�� �־�� �մϴ�)

            enemy = collision.gameObject.transform;

            // ���� �÷��̾� ��ġ�� ����
            enemy.position = Vector2.MoveTowards(enemy.position, player.transform.position, 5f * Time.deltaTime);
            GetComponent<PhotonView>().RPC("EatEnemy", RpcTarget.All);
            //EatEnemy();
            

            if (playerCol != null && enemyCol != null) //�÷��̾�� �浹���� �� �ݶ��̴��� ������� �ʴٸ�
                Physics2D.IgnoreCollision(playerCol, enemyCol, false); //���� ������ ���� �ε����� ����
        }
    }

}