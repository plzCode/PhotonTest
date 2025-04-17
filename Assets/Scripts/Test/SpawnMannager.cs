using System.Diagnostics;
using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Pun;

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
        int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber; //ActorNumber -> 플레이어 번호
        int spawnIndex = (actorNumber - 1) % spawnPoints.Length;

        Transform spawnPoint = spawnPoints[spawnIndex];        
        GameObject tmpPlayer = PhotonNetwork.Instantiate(playerPrefabName, spawnPoint.position, spawnPoint.rotation);

        PhotonView playerView = tmpPlayer.GetComponent<PhotonView>();
        int viewID = playerView.ViewID;

        // 색상 설정 RPC 호출
        playerView.RPC("SettingColor", RpcTarget.AllBuffered, actorNumber, viewID);
    }    

}
