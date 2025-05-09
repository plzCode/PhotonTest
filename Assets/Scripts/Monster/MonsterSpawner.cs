using Photon.Pun;
using System.Collections;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    //public static MonsterSpawner Instance;
    //private void Awake()
    //{
    //    if (Instance != null && Instance != this)
    //    {
    //        Destroy(gameObject);
    //        return;
    //    }

    //    Instance = this;
    //    DontDestroyOnLoad(gameObject);
    //}

    public PhotonView photonView;
    public GameObject[] monsterGroup;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        //if (PhotonNetwork.IsMasterClient)
        //{
        //    StartCoroutine(DelayedDeactivateCall());
        //}

    }
    private IEnumerator DelayedDeactivateCall()
    {
        yield return new WaitForSeconds(3f);

        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("DeactivateSelfAndChildren", RpcTarget.All);
        }
    }

    [PunRPC]
    public void ReSpawnRPC(int viewID)
    {
        PhotonView pview = PhotonView.Find(viewID);
        if (pview != null)
        {
            StartCoroutine(ReSpawner(pview.gameObject));
        }
    }

    public IEnumerator ReSpawner(GameObject obj)
    {
        Debug.Log("���������");
        obj.SetActive(false);
        yield return new WaitForSeconds(3f);
        obj.SetActive(true);
        Debug.Log("�ٽ� ��Ÿ��");
    }

    [PunRPC]
    public void DeactivateSelfAndChildren()
    {
        // ��� ���͸� ���� ��Ȱ��ȭ
        foreach (GameObject monster in monsterGroup)
        {
            if (monster != null)
                monster.SetActive(false);
        }

        foreach (GameObject monster in monsterGroup)
        {
            if (monster != null)
                monster.SetActive(true);
        }
    }

    [PunRPC]
    public void ActFalseWithChildren()
    {
        // ��� ���͸� ���� ��Ȱ��ȭ
        foreach (GameObject monster in monsterGroup)
        {
            if (monster != null)
                monster.SetActive(false);
        }
                
    }
}