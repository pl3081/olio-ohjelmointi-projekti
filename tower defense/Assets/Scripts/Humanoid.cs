using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Humanoid : MonoBehaviour
{
    [SerializeField] int maxHP;
    [SerializeField] public int HP;
    public int MaxHP => maxHP;

    [SerializeField] int attackDamage;
    [SerializeField] float attackRange;
    [SerializeField] float attackSpeed; // in seconds
    float attackCoolDown;               //

    Vector3 lookPos;
    Humanoid attackTarget;
    public Humanoid AttackTarget => attackTarget;

    private void Awake()
    {
        HP = maxHP;
    }

    public bool MoveTo(Vector3 pos)
    {
        ClearAttackTarget();
        return SetDestination(pos);
    }
    public void SetAttackTarget(Humanoid target)
    {
        SetDestination(this.transform.position);
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
        NavMeshAgent navAgent = this.GetComponent<NavMeshAgent>();
        navAgent.SetDestination(pos);
        return true; // todo check if not out of map and ai could walk there
    }
    private bool Attack(Humanoid target)
    {
        if (Vector3.Distance(this.transform.position, target.transform.position) < this.attackRange)
        {
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
                attackTarget = null;
            }
            else
            {
                Attack(attackTarget);
            }
        }
    }
}
