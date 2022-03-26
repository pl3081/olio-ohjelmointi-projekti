using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Unit;

public class Unit : Humanoid, ISmartObject<AI>
{
    [SerializeField] int cost;
    public int Cost => cost;
    private AI _AIController;
    public AI AIController => _AIController;

    public class AI
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
        public AI(Unit unit)
        {
            this.unit = unit;
            navAgent = unit.gameObject.GetComponent<NavMeshAgent>();
        }
        public void SetBehaviour(Behaviour pattern)
        {
            behavPattern = pattern;
        }

        BasicUnit FindNearestEnemy()
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
                return nearestEnemy.GetComponent<BasicUnit>();
        }
        void AttackEnemy()
        {
            if (unit.AttackTarget == null || unit.AttackTarget.Dead || behavPattern == Behaviour.Passive)
            {
                if(behavPattern == Behaviour.Aggressive)
                    unit.StopAction();
                return;
            }    
                
            if (Vector3.Distance(unit.transform.position, unit.AttackTarget.transform.position) < unit.AttackRange)
            {
                if (behavPattern == Behaviour.Aggressive)
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
                BasicUnit newTarget = FindNearestEnemy();
                if (newTarget != null)
                    unit.SetAttackTarget(newTarget);
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
        _AIController = new AI(this);
    }
    protected override void Update()
    {
        base.Update();
        _AIController.Update();
    }
}