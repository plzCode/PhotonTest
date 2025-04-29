using Photon.Pun;
using Unity.Services.Authentication.PlayerAccounts;
using UnityEngine;

public class EatEffect : MonoBehaviour
{
    public Player player;
    public Transform enemy;
    public PhotonView pView;
    public bool isEat;
    public int PormNumber;

    void Start()
    {
        isEat = false;
    }

    void Update()
    {
    }
    /*[PunRPC]
    public void EatEnemy()
    {
        if (enemy == null || player == null) return; //�̹� �԰��ִٸ� ����
        if (Vector2.Distance(player.transform.position, enemy.position) < 1f) //������ �Ÿ��� 1���� �۴ٸ�
        {
            PhotonView eView = enemy.GetComponent<PhotonView>();
            eView.RPC("DestroySelf", eView.Owner);

            //PhotonNetwork.Destroy(enemy.gameObject);  //������
            player.EatKirbyFormNum = PormNumber; //�Դ� Ŀ�� ��ǿ� ���� ���� ��ȣ ���� ����
            player.KirbyFormNum = 1; //�Դ� Ŀ��� ����
            Debug.Log(player.GetComponent<PhotonView>().ViewID + "�� ���� ����"); //���� ����
            player.stateMachine.ChangeState(player.eatState); //�÷��̾� �������            
            //PlayerEetState���� ���� �Լ� �� 
        }
    }*/

    public void OnTriggerStay2D(Collider2D collision) //���� �浹�� �̶��
    {
        //if (!pView.IsMine) return;
        if (collision == null || player == null) return; //�浹���� ���� ���ٸ� ����
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Item")) //�浹���� �� �ݶ��̴��� ������
        {

            Collider2D playerCol = player.GetComponent<Collider2D>(); //�÷��̾� �ݶ��̴��� ������
            Collider2D enemyCol = collision.GetComponent<Collider2D>(); //�浹���� �� �ݶ��̴��� ������
            PormNumber = collision.GetComponent<EnemyNumber>().Number; //�����ϴ� ���� ���� ��ȣ�� �����ȴϴ�. (������ EnemyNumber ��ũ��Ʈ�� �־���ϰ� ��ȣ�� �־�� �մϴ�)

            enemy = collision.gameObject.transform;

            // ���� �÷��̾� ��ġ�� ����
            enemy.position = Vector2.MoveTowards(enemy.position, player.transform.position, 5f * Time.deltaTime);

            if (Vector2.Distance(player.transform.position, enemy.position) < 1f) //������ �Ÿ��� 1���� �۴ٸ�
            {

                Debug.Log("��ƸԴ� �� : " + isEat);
                if (!isEat && PhotonNetwork.IsMasterClient)
                {
                    isEat = true; //�Դ���
                    Debug.Log("�Դ� �� : " + isEat);
                    pView.RPC("EatEnemy", RpcTarget.All, enemy.GetComponent<PhotonView>().ViewID);
                }
            }


            //EatEnemy();            

            if (playerCol != null && enemyCol != null) //�÷��̾�� �浹���� �� �ݶ��̴��� ������� �ʴٸ�
                Physics2D.IgnoreCollision(playerCol, enemyCol, false); //���� ������ ���� �ε����� ����
        }
    }

    public void Eating3State()
    {
        if (player == null) return;
        player.stateMachine.ChangeState(player.eating3State);
    }

    public void Eating4State()
    {
        if (player == null) return;
        player.stateMachine.ChangeState(player.eating4State);
    }

    [PunRPC]
    public void SetPlayer(int playerViewID)
    {
        PhotonView playerView = PhotonView.Find(playerViewID);
        if (playerView != null)
        {
            this.player = playerView.GetComponent<Player>();
        }
    }
}