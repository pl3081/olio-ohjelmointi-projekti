using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrower : Enemy
{
    [SerializeField] Transform hand;
    [SerializeField] float takeRange;
    [SerializeField] float throwRange;
    [SerializeField] float throwDamage;
    [SerializeField] float throwCoolDown;

    protected override void Awake()
    {
        base.Awake();
        SkillList.Add(new TakeSkill(hand, takeRange, throwCoolDown));
        SkillList.Add(new ThrowSkill(hand, throwRange, throwDamage, throwCoolDown));
    }
    protected void Start()
    {
        GameObject[] units = GameObject.FindGameObjectsWithTag("Humanoid");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        print(((TakeSkill)SkillList[0]).Take(units[0].transform));
        print(((ThrowSkill)SkillList[1]).Throw(enemies[0].transform.position));
    }
}
