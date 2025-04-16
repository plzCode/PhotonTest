using System.Diagnostics;
using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Pun;

public class SceneMannager : MonoBehaviour
{
    public static SceneMannager Instance;

    private void Awake()
    {
        // 싱글톤 패턴
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 유지
        }
        else
        {
            Destroy(gameObject);
        }
    }

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
        int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
        int spawnIndex = (actorNumber - 1) % spawnPoints.Length;

        Transform spawnPoint = spawnPoints[spawnIndex];

        PhotonNetwork.Instantiate(playerPrefabName, spawnPoint.position, spawnPoint.rotation);
    }
}
