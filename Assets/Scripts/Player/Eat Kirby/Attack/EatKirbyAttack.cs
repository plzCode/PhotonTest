using UnityEngine;
using Photon.Pun;

public class EatKirbyAttack : MonoBehaviourPun
{
    private Player player => GetComponentInParent<Player>();

    public float moveSpeed = 8f;
    public float lifeTime = 10f; // 10�� �ڿ� ����
    private float timer = 0f;

    void Update()
    {
        transform.Translate(moveSpeed * Time.deltaTime, 0, 0);

        timer += Time.deltaTime;
        if (timer > lifeTime)
        {
            Destroy();
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("������" + player.curAbility.attackPower + "��ŭ �������� ��");
        }

        // Ground ���̾� üũ (�浹���� �� ����)
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy();
        }
    }

    private void Destroy()
    {
        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}