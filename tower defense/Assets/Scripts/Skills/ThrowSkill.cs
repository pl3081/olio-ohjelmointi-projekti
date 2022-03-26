using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowSkill : Skill
{
    private float range;
    private float damage;

    public ThrowSkill(float range, float damage, float coolDown)
    {
        this.range = range;
        this.damage = damage;
        this.coolDown = coolDown;
    }
    protected override void Ability()
    {
        Debug.Log("throw");
    }
}
