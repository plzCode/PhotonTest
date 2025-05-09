
using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Pun;
using Unity.Cinemachine;
using Unity.VisualScripting;
using Unity.Mathematics;

public class SpawnMannager : MonoBehaviourPun
{

    public string gameSceneName = "GameScene";
    public string playerPrefabName = "Test/Player";// Resources ������ �־�� ��    
    public Transform[] spawnPoints; // �̸� �����س��� ���� ������

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == gameSceneName)
        {
            UnityEngine.Debug.Log("GameScene �ε� �Ϸ�. �÷��̾� ���� ����.");
            SpawnMyPlayer();
        }
    }

    void SpawnMyPlayer()
    {
        int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber; // ActorNumber -> �÷��̾� ��ȣ
        int spawnIndex = (actorNumber - 1) % spawnPoints.Length;

        Transform spawnPoint = spawnPoints[spawnIndex];
        GameObject tmpPlayer = PhotonNetwork.Instantiate(playerPrefabName, spawnPoint.position, spawnPoint.rotation);

        PhotonView playerView = tmpPlayer.GetComponent<PhotonView>();
        int viewID = playerView.ViewID;

        // ���� ���� RPC ȣ��
        playerView.RPC("SettingColor", RpcTarget.AllBuffered, actorNumber, viewID);

        // �÷��̾��� ī�޶� ����
        GameObject camObj = new GameObject("PlayerCamera");
        CinemachineCamera pCam = camObj.AddComponent<CinemachineCamera>();
        //pCam.AddComponent<CinemachineFollow>();
        pCam.AddComponent<CinemachineHardLockToTarget>();
        pCam.AddComponent<CinemachinePositionComposer>();
        pCam.AddComponent<CinemachineConfiner2D>();
        pCam.GetComponent<CinemachineConfiner2D>().BoundingShape2D = GameObject.Find("Confiners").GetComponentInChildren<PolygonCollider2D>();



        pCam.Follow = tmpPlayer.transform;
        pCam.LookAt = tmpPlayer.transform;
        pCam.Lens.OrthographicSize = 6f;
        pCam.Lens.NearClipPlane = -1f;

        Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        canvas.worldCamera = pCam.GetComponent<CinemachineCamera>().GetComponent<Camera>();

        camObj.AddComponent<CinemachineBasicMultiChannelPerlin>();
        pCam.GetComponent<CinemachineBasicMultiChannelPerlin>().NoiseProfile = Resources.Load<NoiseSettings>("Bonkers_Shake");
        pCam.GetComponent<CinemachineBasicMultiChannelPerlin>().AmplitudeGain = 0;
        pCam.GetComponent<CinemachineBasicMultiChannelPerlin>().FrequencyGain = 0;
        CameraShake.Instance.cinemCamera = camObj.GetComponent<CinemachineCamera>();
        CameraShake.Instance.noise = camObj.GetComponent<CinemachineBasicMultiChannelPerlin>();

        // ü�� UI ������ ��� Ŭ���̾�Ʈ���� ����
        playerView.RPC("CreateHealthBar", RpcTarget.AllBuffered, actorNumber, viewID);
        // �κ��丮 UI ������ ��� Ŭ���̾�Ʈ���� ����
        playerView.RPC("CreateInventory", RpcTarget.AllBuffered, viewID);
        // �÷��̾��� Area string�� ����
        playerView.RPC("Setting_Area_Name", RpcTarget.AllBuffered, "stage1", viewID,"TestDoor");

    }


}
