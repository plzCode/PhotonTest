using Photon.Pun;
using UnityEngine;

public class Monster_Mole : Enemy
{

    #region States
    public M_Mole_IdleState idleState { get; private set; }
    public M_Mole_BattleState battleState { get; private set; }
    public Spear_HitState hitState { get; private set; }
    #endregion
    protected override void Awake()
    {
        base.Awake();

        idleState = new M_Mole_IdleState(this, stateMachine, "Idle");
        battleState = new M_Mole_BattleState(this, stateMachine, "AttackJump");
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
        else if (stateName == "Battle")
            stateMachine.ChangeState(battleState);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(PhotonNetwork.IsMasterClient&&canAttacking)
        {
            if(collision.gameObject.CompareTag("Player"))
            {
                canAttacking = false;
                collision.gameObject.GetComponent<Player>().pView.RPC("TakeDamage", RpcTarget.All, (Vector2)transform.position, attackPower); // 데미지 처리
                Debug.Log("Mole이 플레이어 에게 " + attackPower + " 만큼 피해를 줌");
            }
        }
    }

}
