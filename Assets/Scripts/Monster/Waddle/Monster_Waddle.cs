using Photon.Pun;
using System.Collections;
using UnityEngine;

public class Monster_Waddle : Enemy
{

    
    #region States
    public Waddle_IdleState idleState { get; private set; }
    public Waddle_MoveState moveState { get; private set; }
    public Spear_HitState hitState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        idleState = new Waddle_IdleState(this, stateMachine, "Idle");
        moveState = new Waddle_MoveState(this, stateMachine, "Move");
        hitState = new Spear_HitState(this, stateMachine, "Hit");
        
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

        if(PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("ChangeState", RpcTarget.All,"Hit");
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
