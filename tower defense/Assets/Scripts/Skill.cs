using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill
{
    protected float coolDown = 0;
    public float CoolDown => coolDown;
    private float usageTime;

    protected virtual bool Use(System.Func<bool> Ability)
    {
        if(Time.realtimeSinceStartup - usageTime >= coolDown)
        {
            if (Ability())
            {
                usageTime = Time.realtimeSinceStartup;
                return true;
            }
        }
        return false;
    }
}
