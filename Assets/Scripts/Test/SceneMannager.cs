using System.Diagnostics;
using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Pun;

public class SceneMannager : MonoBehaviour
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
        int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber; //ActorNumber -> �÷��̾� ��ȣ
        int spawnIndex = (actorNumber - 1) % spawnPoints.Length;

        Transform spawnPoint = spawnPoints[spawnIndex];

        GameObject tmpPlayer = PhotonNetwork.Instantiate(playerPrefabName, spawnPoint.position, spawnPoint.rotation);
        switch(actorNumber)
        {
            case 1:
                tmpPlayer.GetComponentInChildren<SpriteRenderer>().color = new Color(255, 255, 255);
                break;
            case 2:
                tmpPlayer.GetComponentInChildren<SpriteRenderer>().color = new Color(255, 255, 0);
                break;
            case 3:
                tmpPlayer.GetComponentInChildren<SpriteRenderer>().color = new Color(0, 0, 255);
                break;
            case 4:
                tmpPlayer.GetComponentInChildren<SpriteRenderer>().color = new Color(0, 255, 0);
                break;
        }

    }
}
