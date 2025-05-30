using Photon.Pun;
using UnityEngine;

public class Monster_Cat : Enemy
{

    #region States
    public Cat_IdleState idleState { get; private set; }
    public Cat_MoveState moveState { get; private set; }
    public Cat_MoveDownState moveDownState { get; private set; }
    public Cat_HitState hitState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState = new Cat_IdleState(this, stateMachine, "Idle");
        moveState = new Cat_MoveState(this, stateMachine, "Move");
        hitState = new Cat_HitState(this, stateMachine, "Hit");
        moveDownState = new Cat_MoveDownState(this, stateMachine, "MoveDown");

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


    [PunRPC]
    public void ChangeState(string stateName)
    {
        if (stateName == "Idle")
            stateMachine.ChangeState(idleState);
        else if (stateName == "Move")
            stateMachine.ChangeState(moveState);
        else if (stateName == "Hit")
            stateMachine.ChangeState(hitState);
        else if (stateName == "MoveDown")
            stateMachine.ChangeState(moveDownState);
        else if (stateName == "ReSpawn")
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (monsterSpawner != null)
                    monsterSpawner.photonView.RPC("ReSpawnRPC", RpcTarget.All, photonView.ViewID);
                else
                {
                    PhotonNetwork.Destroy(gameObject);
                }

            }
        }
    }

    [PunRPC]
    public override void TakeDamage(float _damage)
    {
        base.TakeDamage(_damage);
        if (currentHp <= 0)
        {
            return;
        }
        currentHp -= _damage;
        if (currentHp <= 0)
        { currentHp = 0; }
        Debug.Log("몬스터가 피해를 " + _damage + "받음");

        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("ChangeState", RpcTarget.All, "Hit");
        }

    }

    [PunRPC]
    public void TurnMoveDown()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("ChangeState", RpcTarget.All, "MoveDown");
        }
    }
    private void OnEnable()
    {
        if (startRight && facingDir == -1)
        {
            Flip();
        }
        else if (!startRight && facingDir == 1)
        {
            Flip();
        }

        if (!isFirstSpawn)
        {
            transform.position = startPosition;
            stateMachine.Initialize(idleState);
        }
        currentHp = maxHp;
    }
}
