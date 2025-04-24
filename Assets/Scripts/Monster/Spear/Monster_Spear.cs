using Photon.Pun;
using UnityEngine;

public class Monster_Spear : Enemy
{
    [Header("M02 ���Ÿ� ���� ����")]
    [SerializeField] public float throwDistance;


    #region States
    public Spear_IdleState idleState { get; private set; }
    public Spear_ThrowState throwState { get; private set; }
    #endregion

    [SerializeField] private GameObject spearPrefab;

    protected override void Awake()
    {
        base.Awake();

        idleState = new Spear_IdleState(this, stateMachine, "Idle", this);
        throwState = new Spear_ThrowState(this, stateMachine, "Throw", this);
    }

    protected override void Start()
    {
        base.Start();

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
    public void ThrowSpear()
    {
        //PhotonNetwork.Instantiate("Monster_Effect/" + spearPrefab.name, transform.position, Quaternion.identity);
        Instantiate(spearPrefab, transform.position, Quaternion.identity);
    }
}
