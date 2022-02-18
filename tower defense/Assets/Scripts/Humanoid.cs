using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Humanoid : MonoBehaviour
{
    [SerializeField] int maxHP;
    [SerializeField] int attackDamage;
    [SerializeField] float attackRange;
    public int HP { get; set; }
    public int MaxHP => maxHP;
    public int AttackDamage => attackDamage;
    public float AttackRange => attackRange;

    public bool MoveTo(Vector3 pos)
    {
        NavMeshAgent navAgent = this.GetComponent<NavMeshAgent>();
        navAgent.SetDestination(pos);
        return true; // todo check if not out of map and ai could walk there
    }
    public bool Attack(Humanoid enemy)
    {
        if (Vector3.Distance(this.transform.position, enemy.transform.position) < this.AttackRange)
        {
            enemy.HP -= this.AttackDamage;
        }
        return true; // todo check if attack available
    }
    public void FaceTarget(Vector3 destination)
    {
        Vector3 lookPos = destination - this.transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, rotation, 0.0035f); // change magic number by rotation speed var
    }
    private bool IsFacedTarget(Vector3 destination)
    {
        float dot = Vector3.Dot(transform.forward, (destination - transform.position).normalized);
        return dot > 0.9f;
    }
}
