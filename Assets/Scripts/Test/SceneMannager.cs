using System.Diagnostics;
using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Pun;

public class SceneMannager : MonoBehaviour
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
        int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber; //ActorNumber -> 플레이어 번호
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
