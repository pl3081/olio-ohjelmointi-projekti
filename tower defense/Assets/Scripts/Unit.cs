using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : Humanoid
{
    [SerializeField] int cost;
    public int Cost => cost;
    public AIControl AI;

    public class AIControl
    {
        GameObject[] enemies => GameObject.FindGameObjectsWithTag("Enemy");
        Unit unit;
        NavMeshAgent navAgent;
        public AIControl(Unit unit)
        {
            this.unit = unit;
            navAgent = unit.gameObject.GetComponent<NavMeshAgent>();
        }
        
        Enemy FindNearestEnemy()
        {
            GameObject nearestEnemy = null;
            float nearestDist = Mathf.Infinity;
            foreach(GameObject enemy in enemies)
            {
                float distToEnemy = Vector3.Distance(unit.transform.position, enemy.transform.position);
                if (distToEnemy < nearestDist)
                {
                    nearestEnemy = enemy;
                    nearestDist = distToEnemy;
                }
            }
            if (nearestEnemy == null)
                return null;
            else
                return nearestEnemy.GetComponent<Enemy>();
        }
        void AttackEnemy()
        {
            if (Vector3.Distance(unit.transform.position, unit.AttackTarget.transform.position) < unit.AttackRange)
            {
                Vector3 vectorXZ = Vector3.forward + Vector3.right;
                if (Vector3.Scale(navAgent.destination, vectorXZ) == Vector3.Scale(unit.AttackTarget.transform.position, vectorXZ))
                {
                    unit.StopMoving();
                }
                unit.FaceTarget(unit.AttackTarget.transform.position);
                unit.Attack();
            }
            else
            {
                navAgent.SetDestination(unit.AttackTarget.transform.position);
            }
        }
        void ChooseAttackTarget()
        {
            float distToEnemy;
            if (unit.AttackTarget != null)
                distToEnemy = Vector3.Distance(unit.transform.position, unit.AttackTarget.transform.position);
            else
                distToEnemy = Mathf.Infinity;

            if (distToEnemy > unit.AttackRange)
            {
                Enemy newTarget = FindNearestEnemy();
                if (newTarget != null)
                    unit.SetAttackTarget(newTarget);
                else
                    unit.StopAction();
            }
        }
        public void Update()
        {
            if(unit.Status == StatusType.Attacking)
            {
                ChooseAttackTarget();
                AttackEnemy();
            }
        }
    };
    protected override void Awake()
    {
        base.Awake();
        AI = new AIControl(this);
    }
    protected override void Update()
    {
        base.Update();
        AI.Update();
    }
}