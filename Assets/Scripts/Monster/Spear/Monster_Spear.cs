using Photon.Pun;
using System.Collections;
using UnityEngine;

public class Monster_Spear : Enemy
{

    
    [Header("M02 ���Ÿ� ���� ����")]
    [SerializeField] public float throwDistance;

    public PhotonView photonView; 
    
    #region States
    public Spear_IdleState idleState { get; private set; }
    public Spear_ThrowState throwState { get; private set; }
    #endregion

    [SerializeField] private GameObject spearPrefab;

    protected override void Awake()
    {
        base.Awake();

        //GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        //player = new Transform[playerObjects.Length];
        //for (int i = 0; i < playerObjects.Length; i++)
        //{
        //    player[i] = playerObjects[i].transform;
        //}


        idleState = new Spear_IdleState(this, stateMachine, "Idle", this);
        throwState = new Spear_ThrowState(this, stateMachine, "Throw", this);

        
    }

    protected override void Start()
    {
        base.Start();

        photonView = GetComponent<PhotonView>();

        stateMachine.Initialize(idleState);

        

    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, throwDistance); // ���� ��Ÿ�
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, throwDistance + 2f); // ���� ��ȯ�� ���� ��Ÿ�
    }


    [PunRPC]
    public void RequestAttackFromClient()
    {
        if(idleState.stateTimer<0)
        {
            stateMachine.ChangeState(throwState);
        }
        
    }

    [PunRPC]
    public void ThrowSpear()
    {
        // ������ Ŭ���̾�Ʈ������ ��â�� ��ȯ�ϵ��� ����
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("ThrowSpear RPC ȣ��� - IsMasterClient: " + PhotonNetwork.IsMasterClient);
            PhotonNetwork.Instantiate("Monster_Effect/" + spearPrefab.name, new Vector2(transform.position.x,transform.position.y+0.1f), Quaternion.identity);
        }
        //Instantiate(spearPrefab, transform.position, Quaternion.identity);
    }

    

    

}
