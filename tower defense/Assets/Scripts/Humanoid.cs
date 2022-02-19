using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Humanoid : MonoBehaviour
{
    [SerializeField] int maxHP;
    public int HP { get; set; }
    public int MaxHP => maxHP;

    [SerializeField] int attackDamage;
    [SerializeField] float attackRange;
    [SerializeField] float attackSpeed; // in seconds
    float attackCoolDown;               //
    public Vector3 GroupPosOffset { get; set; } // formation

    Vector3 destination;
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


    private void Awake()
    {
        HP = maxHP;
        status = StatusType.Stopped;
        navAgent = this.GetComponent<NavMeshAgent>();
    }

    public bool MoveTo(Vector3 pos)
    {
        status = StatusType.Moving;
        ClearAttackTarget();
        return SetDestination(pos);
    }
    public void Stop()
    {
        navAgent.SetDestination(this.transform.position);
        destination = Vector3.zero;
    }
    public void SetAttackTarget(Humanoid target)
    {
        status = StatusType.Attacking;
        Stop();
        attackTarget = target;
    }
    public void ClearAttackTarget()
    {
        lookPos = Vector3.zero;
        attackTarget = null;
    }
    public void FaceTarget(Vector3 destination)
    {
        lookPos = destination - this.transform.position;
    }

    private bool SetDestination(Vector3 pos)
    {
        destination = pos;
        Vector3 offset = Quaternion.LookRotation((pos + GroupPosOffset - this.transform.position).normalized) * GroupPosOffset;
        navAgent.SetDestination(pos + offset);
        return true; // todo check if not out of map and ai could walk there
    }
    private bool Attack(Humanoid target)
    {
        if (Vector3.Distance(this.transform.position, target.transform.position) < this.attackRange)
        {
            if(this.destination == target.transform.position)
            {
                this.Stop();
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
            SetDestination(target.transform.position);
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

    private void Update()
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
                status = StatusType.Stopped;
                attackTarget = null;
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
                destination = Vector3.zero;
            }
        }
    }
}
