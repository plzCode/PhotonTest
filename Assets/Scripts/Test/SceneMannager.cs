using System.Diagnostics;
using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Pun;

public class SceneMannager : MonoBehaviour
{
    public static SceneMannager Instance;

    private void Awake()
    {
        // �̱��� ����
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �� ��ȯ �ÿ��� ����
        }
        else
        {
            Destroy(gameObject);
        }
    }

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
        int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
        int spawnIndex = (actorNumber - 1) % spawnPoints.Length;

        Transform spawnPoint = spawnPoints[spawnIndex];

        PhotonNetwork.Instantiate(playerPrefabName, spawnPoint.position, spawnPoint.rotation);
    }
}
