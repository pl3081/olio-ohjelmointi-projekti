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
    public virtual AI AIController => _AIController;

    public class AI
    {
        public enum Behaviour
        {
            Aggressive,
            Defensive,
            Passive
        }
        protected Behaviour behavPattern = Behaviour.Defensive;

        protected List<GameObject> enemies => new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
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

        protected virtual BasicUnit FindNearestUnit(List<GameObject> units)
        {
            GameObject nearestUnit = null;
            float nearestDist = Mathf.Infinity;
            foreach(GameObject obj in units)
            {
                if (obj == unit.gameObject)
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
        protected void InteractWithUnitAtRange(bool cond, BasicUnit target, float range, Func<BasicUnit,bool> action, Action elseAction)
        {
            if (!cond)
            {
                elseAction();
                return;
            }
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
            void elseAction()
            {
                if (behavPattern == Behaviour.Aggressive)
                    unit.StopAction();
            }
            InteractWithUnitAtRange(!cantAttack, unit.AttackTarget, unit.AttackRange, unit.Attack, elseAction);
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
                BasicUnit newTarget = FindNearestUnit(enemies);
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
    }
    protected override void Update()
    {
        base.Update();
        _AIController.Update();
    }
}