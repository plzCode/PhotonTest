using Photon.Pun;
using UnityEngine;

public class PlayerTest : MonoBehaviourPunCallbacks, IPunObservable
{
    PhotonView playerView;

    private void Start()
    {
        playerView = GetComponent<PhotonView>();
    }

/*    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // ���� �÷��̾��� �����͸� ��Ʈ��ũ�� ����
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            // ��Ʈ��ũ���� �����͸� ����
            transform.position = (Vector3)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }*/

    private void FixedUpdate()
    {
        
    }
    [PunRPC]
    void SettingColor(int number, int viewID)
    {
        PhotonView targetView = PhotonView.Find(viewID);
        if (targetView == null) return;

        SpriteRenderer sr = targetView.GetComponentInChildren<SpriteRenderer>();
        if (sr == null) return;

        switch (number)
        {
            case 1:
                sr.color = new Color32(255, 255, 255, 255);
                break;
            case 2:
                sr.color = new Color32(255, 255, 0, 255);
                break;
            case 3:
                sr.color = new Color32(0, 0, 255, 255);
                break;
            case 4:
                sr.color = new Color32(0, 255, 0, 255);
                break;
        }
    }

    [PunRPC]
    public void CreateHealthBar(int number, int playerViewID)
    {
        PhotonView playerView = PhotonView.Find(playerViewID);
        if (playerView == null)
        {
            Debug.LogError($"Player with ViewID {playerViewID} not found.");
            return;
        }

        Player player = playerView.GetComponent<Player>();
        if (player == null)
        {
            Debug.LogError("Player component not found on the target object.");
            return;
        }

        Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Canvas not found in the scene.");
            return;
        }

        // ü�� UI ����
        GameObject healthBar = Instantiate(Resources.Load("UI/HealthBar" + number.ToString()), canvas.transform) as GameObject;
        healthBar.transform.SetParent(canvas.transform);

        // UI ��ġ ���� (�÷��̾� ����Ʈ�� ���� ��ġ ����)
        float verticalSpacing = Screen.height * 0.14f; // ȭ�� ������ 14%�� �������� ���
        float i = verticalSpacing * GameManager.Instance.playerList.Count;
        healthBar.transform.position = new Vector3(Screen.width * 0.984f, Screen.height * 0.945f - i, 0);

        // Health_Bar ����
        Health_Bar healthBarScript = healthBar.GetComponentInChildren<Health_Bar>();
        healthBarScript.UpdateHealthBar(100f);
        healthBarScript.SetPlayer(player);
        player.health_Bar = healthBarScript;
        healthBarScript.player = player; // �÷��̾� ��ũ��Ʈ�� ����
    }

    [PunRPC]
    public void CreateInventory(int playerViewID)
    {
        PhotonView playerView = PhotonView.Find(playerViewID);
        // �κ��丮 ������ �ε� �� ��ȯ
        GameObject inventoryPrefab = Resources.Load<GameObject>("UI/Inventory");
        if (inventoryPrefab == null)
        {
            Debug.LogError("Inventory prefab not found in Resources/UI.");
            return;
        }
        Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        GameObject inventoryInstance = Instantiate(inventoryPrefab, canvas.transform);
        Inventory inventoryScript = inventoryInstance.GetComponent<Inventory>();

        if (inventoryScript == null)
        {
            Debug.LogError("Inventory component not found on the instantiated prefab.");
            return;
        }

        // ���� �÷��̾ �ƴ� ��� Inventory ��Ȱ��ȭ
        if (!playerView.IsMine)
        {
            inventoryInstance.SetActive(false);
        }

        // �κ��丮 ���
        GetComponent<Player>().inventory = inventoryScript;
        inventoryScript.player = GetComponent<Player>();
    }
}

