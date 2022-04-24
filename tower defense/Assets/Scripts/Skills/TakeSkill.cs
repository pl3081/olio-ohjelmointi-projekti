using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TakeSkill : Skill
{
    Transform hand;
    float range;
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
        if(Vector3.Distance(hand.position, objectToTake.position) <= range && hand.childCount == 0)
        {
            objectToTake.position = hand.position;
            objectToTake.SetParent(hand);
            objectToTake.GetComponent<NavMeshAgent>().isStopped = true;
            return true;
        }
        return false;
    }
}