using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicUnit : MonoBehaviour, IDestructableObject
{
    protected HealthBar healthBar;
    protected Vector3 healthBarOffset = new Vector3(0, 4, 0);

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
            if (value > maxHP)
            {
                _hp = maxHP;
            }
            else if (value > 0)
            {
                _hp = value;
            }
            else
            {
                Die();
                _hp = 0;
            }
            if(!Dead)
                healthBar.Value = _hp;
        }
    }
    public bool Dead => HP <= 0;
    protected virtual void Die()
    {
        gameObject.tag = "Corpse";
        animator.SetTrigger(DeathHash);
        healthBar.Delete();
        Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
    }
    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        healthBar = new HealthBar(MaxHP);
        _hp = MaxHP;
    }
    protected virtual void Update()
    {
        if(!Dead)
            healthBar.SetPosition(Camera.main.WorldToScreenPoint(this.transform.position + healthBarOffset));
    }
}
