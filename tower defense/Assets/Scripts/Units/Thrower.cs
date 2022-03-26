using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrower : Enemy
{
    [SerializeField] float throwRange;
    [SerializeField] float throwDamage;
    [SerializeField] float throwCoolDown;

    protected override void Awake()
    {
        base.Awake();
        SkillList.Add(new ThrowSkill(throwRange, throwDamage, throwCoolDown));
    }
}
