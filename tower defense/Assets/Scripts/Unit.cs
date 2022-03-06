using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : Humanoid
{
    [SerializeField] int cost;
    public int Cost => cost;
    public UnitAI AI;

    public class UnitAI
    {
        public enum Behaviour
        {
            Aggressive,
            Defensive,
            Passive
        }
        Behaviour behavPattern = Behaviour.Defensive;
        GameObject[] enemies => GameObject.FindGameObjectsWithTag("Enemy");
        Unit unit;
        NavMeshAgent navAgent;
        public UnitAI(Unit unit)
        {
            this.unit = unit;
            navAgent = unit.gameObject.GetComponent<NavMeshAgent>();
        }
        public void SetBehaviour(Behaviour pattern)
        {
            behavPattern = pattern;
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
            if (unit.AttackTarget == null || behavPattern == Behaviour.Passive)
                return;
            if (Vector3.Distance(unit.transform.position, unit.AttackTarget.transform.position) < unit.AttackRange)
            {
                if (unit.IsDestination(unit.AttackTarget.transform.position))
                {
                    unit.StopMoving();
                }
                unit.FaceTarget(unit.AttackTarget.transform.position);
                unit.Attack();
            }
            else if(behavPattern == Behaviour.Aggressive)
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
            if(behavPattern == Behaviour.Defensive || behavPattern == Behaviour.Aggressive)
            {
                ChooseAttackTarget();
                AttackEnemy();
            }
            
        }
    };
    protected override void Awake()
    {
        base.Awake();
        AI = new UnitAI(this);
    }
    protected override void Update()
    {
        base.Update();
        AI.Update();
    }
}