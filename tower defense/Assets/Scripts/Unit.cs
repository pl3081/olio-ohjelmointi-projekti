using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using static Unit;

public abstract class Unit : Humanoid, ISmartObject<AI>
{
    [SerializeField] int cost;
    public int Cost => cost;
    private AI _AIController;
    static readonly int SpeedHash = Animator.StringToHash("AttackSpeed");
    public virtual AI AIController => _AIController;
    protected virtual List<Unit> Enemies => Area.Enemies;

    public class AI
    {
        public enum Behaviour
        {
            Aggressive,
            Defensive,
            Passive
        }
        protected Behaviour behavPattern = Behaviour.Defensive;
        private Unit unit;
        protected NavMeshAgent navAgent;

        protected virtual bool cantAttack => unit.AttackTarget == null || unit.AttackTarget.Dead || behavPattern == Behaviour.Passive;

        public AI(Unit unit)
        {
            this.unit = unit;
            navAgent = unit.gameObject.GetComponent<NavMeshAgent>();
        }
        public void SetBehaviour(Behaviour pattern)
        {
            this.behavPattern = pattern;
        }

        protected virtual BasicUnit FindNearestUnit(List<Unit> units)
        {
            GameObject nearestUnit = null;
            float nearestDist = Mathf.Infinity;
            foreach(Unit selectedUnit in units)
            {
                GameObject obj = selectedUnit.gameObject;
                if (selectedUnit == unit)
                    continue;
                float distToUnit = Vector3.Distance(unit.transform.position, obj.transform.position);
                if (distToUnit < nearestDist)
                {
                    nearestUnit = obj;
                    nearestDist = distToUnit;
                }
            }
            if (nearestUnit == null)
                return null;
            else
                return nearestUnit.GetComponent<BasicUnit>();
        }
        protected void InteractWithUnitAtRange(BasicUnit target, float range, Func<BasicUnit,bool> action)
        {
            if (target == null) return;
            if (Vector3.Distance(unit.transform.position, target.transform.position) < range)
            {
                if (behavPattern == Behaviour.Aggressive)
                {
                    unit.StopMoving();
                }
                unit.FaceTarget(target.transform.position);
                action(target);
            }
            else if (behavPattern == Behaviour.Aggressive)
            {
                unit.MoveTo(target.transform.position);
            }
        }
        protected virtual void AttackEnemy()
        {
            if (cantAttack)
            {
                if (behavPattern == Behaviour.Aggressive)
                    unit.StopAction();
            }
            else
                InteractWithUnitAtRange(unit.AttackTarget, unit.AttackRange, unit.Attack);
        }
        protected virtual void ChooseAttackTarget()
        {
            float distToEnemy;
            if (unit.AttackTarget != null)
                distToEnemy = Vector3.Distance(unit.transform.position, unit.AttackTarget.transform.position);
            else
                distToEnemy = Mathf.Infinity;

            if (distToEnemy > unit.AttackRange)
            {
                BasicUnit newTarget = FindNearestUnit(unit.Enemies);
                if (newTarget != null)
                    unit.SetAttackTarget(newTarget);
            }
        }
        public virtual void Update()
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
        animator.SetFloat(SpeedHash, AttackSpeed);
    }
    protected override void Update()
    {
        base.Update();
        _AIController.Update();
    }
    
    protected override void Die()
    {
        base.Die();
        Area.ProcessDeath(this);
    }
}