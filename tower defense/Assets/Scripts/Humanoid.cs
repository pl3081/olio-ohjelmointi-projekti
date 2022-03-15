using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Humanoid : BasicUnit, IMovingObject, IAttackingObject
{
    [SerializeField] int attackDamage;
    [SerializeField] float attackRange;
    [SerializeField] float attackSpeed; // in seconds
    public int AttackDamage => attackDamage;
    public float AttackRange => attackRange;
    public float AttackSpeed => attackSpeed;

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
    public bool IsDestination(Vector3 pos)
    {
        Vector3 vectorXZ = Vector3.forward + Vector3.right;
        Vector3 destination = Vector3.Scale(navAgent.destination, vectorXZ);
        Vector3 targetPos = Vector3.Scale(pos, vectorXZ);
        return destination == targetPos;
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
    
    protected override void Awake()
    {
        base.Awake();
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
            if (attackTarget.Dead)
            {
                SetAttackTarget(null);
            }
        }
        if(status == StatusType.Moving)
        {
            if (IsDestination(this.transform.position))
            {
                status = StatusType.Stopped;
            }
        }
    }
}
