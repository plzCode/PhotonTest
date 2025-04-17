using Photon.Pun;
using UnityEngine;

public class Tmp_LeaveRoom : MonoBehaviourPunCallbacks
{
    public void Leave_Room()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveLobby();
            LoadLobbyScene(); // �κ� ������ �̵�  
            Debug.Log("Left Room: " + PhotonNetwork.CurrentRoom.Name);
        }
        else
        {
            Debug.Log("Not in a room");
        }
    }

    private void LoadLobbyScene()
    {
        // SceneManagerHelper.ActiveSceneName�� �б� �����̹Ƿ�,  
        // SceneManager.LoadScene()�� ����Ͽ� �κ� ���� �ε��մϴ�.  
        UnityEngine.SceneManagement.SceneManager.LoadScene("TestPhoton");
    }
}
