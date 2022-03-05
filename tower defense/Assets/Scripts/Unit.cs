using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : Humanoid
{
    [SerializeField] int cost;
    public int Cost => cost;
    public AIControl AI;

    public class AIControl
    {
        GameObject[] enemies => GameObject.FindGameObjectsWithTag("Enemy");
        Unit unit;
        public AIControl(Unit unit)
        {
            this.unit = unit;
        }
        
        Enemy FindNearestEnemy()
        {
            print(enemies.Length);
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
        public void Update()
        {
            if(unit.Status == StatusType.Attacking)
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
                        unit.Stop();
                }
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