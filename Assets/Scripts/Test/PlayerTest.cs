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
            // 로컬 플레이어의 데이터를 네트워크로 전송
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            // 네트워크에서 데이터를 수신
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

        // 체력 UI 생성
        GameObject healthBar = Instantiate(Resources.Load("UI/HealthBar" + number.ToString()), canvas.transform) as GameObject;
        healthBar.transform.SetParent(canvas.transform);

        // UI 위치 조정 (플레이어 리스트에 따라 위치 변경)
        float verticalSpacing = Screen.height * 0.14f; // 화면 높이의 14%를 간격으로 사용
        float i = verticalSpacing * GameManager.Instance.playerList.Count;
        healthBar.transform.position = new Vector3(Screen.width * 0.984f, Screen.height * 0.945f - i, 0);

        // Health_Bar 설정
        Health_Bar healthBarScript = healthBar.GetComponentInChildren<Health_Bar>();
        healthBarScript.UpdateHealthBar(100f);
        healthBarScript.SetPlayer(player);
        player.health_Bar = healthBarScript;
        healthBarScript.player = player; // 플레이어 스크립트와 연결
    }

    [PunRPC]
    public void CreateInventory(int playerViewID)
    {
        PhotonView playerView = PhotonView.Find(playerViewID);
        // 인벤토리 프리팹 로드 및 소환
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

        // 로컬 플레이어가 아닌 경우 Inventory 비활성화
        if (!playerView.IsMine)
        {
            inventoryInstance.SetActive(false);
        }

        // 인벤토리 등록
        GetComponent<Player>().inventory = inventoryScript;
        inventoryScript.player = GetComponent<Player>();
    }
}

