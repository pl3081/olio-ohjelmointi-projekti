using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Humanoid : MonoBehaviour
{
    [SerializeField] int maxHP;
    [SerializeField] int attackDamage;
    [SerializeField] float attackRange;
    [SerializeField] float attackSpeed; // in seconds
    public int MaxHP => maxHP;
    public int AttackDamage => attackDamage;
    public float AttackRange => attackRange;
    public float AttackSpeed => attackSpeed;
    public int HP;
    
    float attackCoolDown;

    Vector3 lookPos;
    Humanoid attackTarget;
    public Humanoid AttackTarget => attackTarget;

    public enum StatusType
    {
        Stopped,
        Moving,
        Attacking
    }
    private StatusType status;
    public StatusType Status => status;
    NavMeshAgent navAgent;

    public bool MoveTo(Vector3 pos)
    {
        StopAction();
        status = StatusType.Moving;
        navAgent.SetDestination(pos);
        return true;
    }
    public void SetAttackTarget(Humanoid target)
    {
        status = StatusType.Attacking;
        attackTarget = target;
    }
    public void FaceTarget(Vector3 destination)
    {
        lookPos = destination - this.transform.position;
    }
    public void StopAction()
    {
        status = StatusType.Stopped;
        attackTarget = null;
        StopMoving();
    }
    public void StopMoving()
    {
        navAgent.SetDestination(this.transform.position);
        lookPos = Vector3.zero;
    }
    public bool Attack()
    {
        bool inRange = Vector3.Distance(AttackTarget.transform.position, this.transform.position) < AttackRange;
        if (attackCoolDown <= 0 && IsFacedTarget(AttackTarget.transform.position) && inRange)
        {
            AttackTarget.HP -= this.attackDamage;
            this.attackCoolDown = this.attackSpeed;
            return true;
        }
        return false;
    }

    private void RotateToDir(Vector3 dir)
    {
        dir.y = 0;
        Quaternion rotation = Quaternion.LookRotation(dir);
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, rotation, 2f * Time.deltaTime); // todo change magic number by rotation speed var
    }
    private bool IsFacedTarget(Vector3 destination)
    {
        float dot = Vector3.Dot(transform.forward, (destination - transform.position).normalized);
        return dot > 0.95f;
    }

    protected virtual void Awake()
    {
        HP = maxHP;
        status = StatusType.Stopped;
        navAgent = GetComponent<NavMeshAgent>();
    }
    protected virtual void Update()
    {
        if(lookPos != Vector3.zero && !IsFacedTarget(lookPos))
        {
            RotateToDir(lookPos);
        }
        if (attackCoolDown > 0)
        {
            attackCoolDown -= Time.deltaTime;
        }
        if (attackTarget != null)
        {
            if (attackTarget.HP <= 0)
            {
                Destroy(AttackTarget.gameObject); // todo create dead body
                SetAttackTarget(null);
            }
        }
        if(status == StatusType.Moving)
        {
            if (this.transform.position == this.navAgent.destination)
            {
                status = StatusType.Stopped;
            }
        }
    }
}
