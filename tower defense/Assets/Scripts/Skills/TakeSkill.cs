using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeSkill : Skill
{
    Transform hand;
    float range;
    Transform objectTaken;
    public Transform ObjectTaken => ObjectTaken;
    public TakeSkill(Transform hand, float range, float coolDown = 0f)
    {
        this.hand = hand;
        this.range = range;
        this.coolDown = coolDown;
    }
    public bool Take(Transform objectToTake)
    {
        return Use(() => Ability(objectToTake));
    }
    private bool Ability(Transform objectToTake)
    {
        if(Vector3.Distance(hand.position, objectToTake.position) <= range && objectTaken == null)
        {
            objectToTake.SetParent(hand);
            objectToTake.position = hand.position;
            objectTaken = objectToTake;
            return true;
        }
        return false;
    }
}