using Photon.Pun;
using UnityEngine;

public class Tmp_LeaveRoom : MonoBehaviourPunCallbacks
{
    public void Leave_Room()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveLobby();
            LoadLobbyScene(); // 로비 씬으로 이동  
            Debug.Log("Left Room: " + PhotonNetwork.CurrentRoom.Name);
        }
        else
        {
            Debug.Log("Not in a room");
        }
    }

    private void LoadLobbyScene()
    {
        // SceneManagerHelper.ActiveSceneName은 읽기 전용이므로,  
        // SceneManager.LoadScene()을 사용하여 로비 씬을 로드합니다.  
        UnityEngine.SceneManagement.SceneManager.LoadScene("TestPhoton");
    }
}
