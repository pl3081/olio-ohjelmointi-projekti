using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Thrower : Unit
{
    [SerializeField] Transform hand;
    [SerializeField] float takeRange;
    [SerializeField] float throwRange;
    [SerializeField] int throwDamage;
    [SerializeField] float throwCoolDown;
    private ThrowerAI _AIController;
    public override AI AIController => _AIController;

    public class ThrowerAI : AI
    {
        protected List<GameObject> allies = new List<GameObject>();
        private Thrower unit;
        public ThrowerAI(Thrower unit) : base(unit)
        {
            this.unit = unit;

            GameObject[] allAllies = GameObject.FindGameObjectsWithTag("Humanoid");
            foreach (GameObject ally in allAllies)
            {
                if (ally.GetComponent<Thrower>() == null)
                    allies.Add(ally);
            }
        }
        void TakeNearestUnit()
        {
            InteractWithUnitAtRange(unit.hand.childCount == 0, FindNearestUnit(allies), unit.takeRange, unit.Take, () => { });
        }
        void ThrowToEnemy()
        {
            bool cantThrow = cantAttack || unit.hand.childCount == 0;
            InteractWithUnitAtRange(!cantThrow, unit.AttackTarget, unit.throwRange, unit.Throw, () => { });
        }
        public override void Update()
        {
            base.Update();
            if (behavPattern == Behaviour.Defensive || behavPattern == Behaviour.Aggressive)
            {
                TakeNearestUnit();
                ThrowToEnemy();
            }
        }
    }
    public bool Throw(Vector3 position)
    {
        if (IsFacedTarget(position))
        {
            if (((ThrowSkill)SkillList[1]).Throw(position))
            {
                return true;
            }
        }
        return false;
    }
    public bool Throw(BasicUnit unit)
    {
        return Throw(unit.transform.position);
    }
    public bool Take(Transform unit)
    {
        if (IsFacedTarget(unit.position))
        {
            if (((TakeSkill)SkillList[0]).Take(unit))
            {
                return true;
            }
        }
        return false;
    }
    public bool Take(BasicUnit unit)
    {
        return Take(unit.transform);
    }
    protected override void Awake()
    {
        base.Awake();
        SkillList.Add(new TakeSkill(hand, takeRange, throwCoolDown));
        SkillList.Add(new ThrowSkill(hand, "Enemy", throwRange, throwDamage, throwCoolDown));
        _AIController = new ThrowerAI(this);
    }
    protected override void Update()
    {
        base.Update();
        _AIController.Update();
    }
}
