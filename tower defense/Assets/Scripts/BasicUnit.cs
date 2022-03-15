using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicUnit : MonoBehaviour, IDestructableObject
{
    protected Animator animator;
    protected static readonly int DeathHash = Animator.StringToHash("Death");

    [SerializeField] protected int maxHP;
    public int MaxHP => maxHP;

    [SerializeField] protected int _hp;
    public int HP
    {
        get => _hp;
        set
        {
            if (value > 0)
            {
                _hp = value;
            }
            else if (value > maxHP)
            {
                _hp = maxHP;
            }
            else
            {
                Die();
                _hp = 0;
            }

        }
    }
    public bool Dead => HP <= 0;
    protected virtual void Die()
    {
        gameObject.tag = "Corpse";
        animator.SetTrigger(DeathHash);
        Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
    }

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        HP = MaxHP;
    }
}
