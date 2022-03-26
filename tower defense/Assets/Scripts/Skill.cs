using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill
{
    protected float coolDown;
    public float CoolDown => coolDown;
    private float usageTime;

    public void Use()
    {
        if(Time.realtimeSinceStartup - usageTime >= coolDown)
        {
            usageTime = Time.realtimeSinceStartup;
            Ability();
        }
    }
    protected virtual void Ability() { } // should be overriden
}
