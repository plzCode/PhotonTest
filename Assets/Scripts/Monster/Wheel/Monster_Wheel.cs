using Photon.Pun;
using UnityEngine;

public class Monster_Wheel : Enemy
{
    #region States
    public Wheel_MoveState moveState { get; private set; }
    public Wheel_TurnState turnState { get; private set; }
    public Wheel_HitState hitState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        moveState = new Wheel_MoveState(this, stateMachine, "Move");
        turnState = new Wheel_TurnState(this, stateMachine, "Turn");
        hitState = new Wheel_HitState(this, stateMachine, "Hit");

    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(moveState);

    }

    protected override void Update()
    {
        base.Update();
    }


    [PunRPC]
    public void ChangeState(string stateName)
    {
        if (stateName == "Move")
            stateMachine.ChangeState(moveState);
        else if (stateName == "Turn")
            stateMachine.ChangeState(turnState);
        else if (stateName == "Hit")
            stateMachine.ChangeState(hitState);

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
    public void WhellTurn()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("ChangeState", RpcTarget.All, "Move");
        }
    }
}
