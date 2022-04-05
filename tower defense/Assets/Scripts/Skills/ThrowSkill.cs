using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ThrowSkill : Skill
{
    private string damagedTag;
    private float range;
    private float splashRange;
    private int damage;
    private Transform hand;

    public ThrowSkill(Transform hand, string damagedTag, float range, int damage, float splashRange, float coolDown = 0f)
    {
        this.hand = hand;
        this.damagedTag = damagedTag;
        this.range = range;
        this.damage = damage;
        this.splashRange = splashRange;
        this.coolDown = coolDown;
    }
    public bool Throw(Vector3 position)
    {
        return Use(() => Ability(position));
    }
    private bool Ability(Vector3 targetPoint)
    {
        if(hand.childCount != 0)
        {
            Transform itemTransform = hand.GetChild(0);
            itemTransform.parent = null;
            itemTransform.gameObject.AddComponent<ThrowedUnit>().Init(damagedTag, damage, splashRange);
            Player.Instance.StartCoroutine(throwItem(itemTransform, targetPoint, 2.0f, 8f));
            return true;
        }
        return false;
    }
    private AnimationCurve[] buildThrowTrajectory(Vector3 origin, Vector3 target, float throwTime, float throwHeight)
    {
        // Returns array of 3 AnimationCurves: 0/1/2 = X/Y/Z.
        
        // Initalise trajectory array.
        AnimationCurve[] trajectory = new AnimationCurve[3];
        trajectory[0] = new AnimationCurve();
        trajectory[1] = new AnimationCurve();
        trajectory[2] = new AnimationCurve();

        // Start point.
        trajectory[0].AddKey(0.0f, origin.x);
        trajectory[1].AddKey(0.0f, origin.y);
        trajectory[2].AddKey(0.0f, origin.z);

        // Mid point.
        trajectory[0].AddKey(throwTime * 0.5f, (origin.x + target.x) * 0.5f);
        trajectory[1].AddKey(throwTime * 0.5f, (origin.y + target.y) * 0.5f + throwHeight);
        trajectory[2].AddKey(throwTime * 0.5f, (origin.z + target.z) * 0.5f);

        // End point.
        trajectory[0].AddKey(throwTime, target.x);
        trajectory[1].AddKey(throwTime, target.y);
        trajectory[2].AddKey(throwTime, target.z);

        return trajectory;
    }
    // Do the actual position and scale transforms for the throwing.
    private IEnumerator throwItem(Transform item, Vector3 targetPoint, float throwLength, float throwHeight)
    {
        bool itemInAir = true;
        float itemTravelTime = 0.0f;

        var trajectory = buildThrowTrajectory(item.position, targetPoint, throwLength, throwHeight);

        while (itemInAir)
        {
            Vector3 newPosition = new Vector3(trajectory[0].Evaluate(itemTravelTime),
                                              trajectory[1].Evaluate(itemTravelTime),
                                              trajectory[2].Evaluate(itemTravelTime));
            item.position = newPosition;

            itemTravelTime += Time.deltaTime;
            if (itemTravelTime > throwLength)
            {
                itemInAir = false;
            }

            yield return null;
        }

        // Just to ensure that item is definitely at the target position after throw.
        item.position = targetPoint;

        item.GetComponent<ThrowedUnit>().Land();
    }
}
