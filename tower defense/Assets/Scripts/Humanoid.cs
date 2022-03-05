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
        Stop();
        status = StatusType.Moving;
        navAgent.SetDestination(pos);
        return true;
    }
    public void Stop()
    {
        status = StatusType.Stopped;
        attackTarget = null;
        StopMoving();
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

    private void StopMoving()
    {
        navAgent.SetDestination(this.transform.position);
        lookPos = Vector3.zero;
    }
    private bool Attack(Humanoid target)
    {
        if (Vector3.Distance(this.transform.position, target.transform.position) < this.attackRange)
        {
            Vector3 vectorXZ = Vector3.forward + Vector3.right;
            if (Vector3.Scale(navAgent.destination, vectorXZ) == Vector3.Scale(target.transform.position, vectorXZ))
            {
                StopMoving();
            }
            FaceTarget(target.transform.position);
            if (attackCoolDown <= 0 && IsFacedTarget(target.transform.position))
            {
                target.HP -= this.attackDamage;
                this.attackCoolDown = this.attackSpeed;
                return true;
            }
        }
        else
        {
            navAgent.SetDestination(target.transform.position);
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
            else
            {
                Attack(attackTarget);
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
