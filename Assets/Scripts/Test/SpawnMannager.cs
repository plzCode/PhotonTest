using System.Diagnostics;
using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Pun;

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
        int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber; //ActorNumber -> �÷��̾� ��ȣ
        int spawnIndex = (actorNumber - 1) % spawnPoints.Length;

        Transform spawnPoint = spawnPoints[spawnIndex];        
        GameObject tmpPlayer = PhotonNetwork.Instantiate(playerPrefabName, spawnPoint.position, spawnPoint.rotation);

        PhotonView playerView = tmpPlayer.GetComponent<PhotonView>();
        int viewID = playerView.ViewID;

        // ���� ���� RPC ȣ��
        playerView.RPC("SettingColor", RpcTarget.AllBuffered, actorNumber, viewID);
    }    

}
