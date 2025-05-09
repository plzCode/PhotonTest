
using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Pun;
using Unity.Cinemachine;
using Unity.VisualScripting;
using Unity.Mathematics;

public class SpawnMannager : MonoBehaviourPun
{

    public string gameSceneName = "GameScene";
    public string playerPrefabName = "Test/Player";// Resources 폴더에 있어야 함    
    public Transform[] spawnPoints; // 미리 지정해놓은 스폰 지점들

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
            UnityEngine.Debug.Log("GameScene 로드 완료. 플레이어 스폰 시작.");
            SpawnMyPlayer();
        }
    }

    void SpawnMyPlayer()
    {
        int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber; // ActorNumber -> 플레이어 번호
        int spawnIndex = (actorNumber - 1) % spawnPoints.Length;

        Transform spawnPoint = spawnPoints[spawnIndex];
        GameObject tmpPlayer = PhotonNetwork.Instantiate(playerPrefabName, spawnPoint.position, spawnPoint.rotation);

        PhotonView playerView = tmpPlayer.GetComponent<PhotonView>();
        int viewID = playerView.ViewID;

        // 색상 설정 RPC 호출
        playerView.RPC("SettingColor", RpcTarget.AllBuffered, actorNumber, viewID);

        // 플레이어의 카메라 설정
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

        // 체력 UI 설정을 모든 클라이언트에서 실행
        playerView.RPC("CreateHealthBar", RpcTarget.AllBuffered, actorNumber, viewID);
        // 인벤토리 UI 설정을 모든 클라이언트에서 실행
        playerView.RPC("CreateInventory", RpcTarget.AllBuffered, viewID);
        // 플레이어의 Area string을 설정
        playerView.RPC("Setting_Area_Name", RpcTarget.AllBuffered, "stage1", viewID,"TestDoor");

    }


}
