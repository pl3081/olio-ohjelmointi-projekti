using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ThrowedUnit : MonoBehaviour
{
    string damagedTag;
    int splashDamage;
    float splashRange;
    public void Init(string damagedTag, int splashDamage, float splashRange)
    {
        this.damagedTag = damagedTag;
        this.splashDamage = splashDamage;
        this.splashRange = splashRange;
        GetComponent<BoxCollider>().enabled = true;
    }
    public void Land()
    {
        GetComponent<NavMeshAgent>().enabled = true;
        GetComponent<BasicUnit>().enabled = true;
        GameObject[] damagedUnits = GameObject.FindGameObjectsWithTag(damagedTag);
        foreach (GameObject unit in damagedUnits)
        {
            if (unit.GetComponent<BasicUnit>() != null)
            {
                if (Vector3.Distance(transform.position, unit.transform.position) <= splashRange)
                {
                    unit.GetComponent<BasicUnit>().HP -= splashDamage;
                }
            }
        }
        Destroy(this);
    }
}
