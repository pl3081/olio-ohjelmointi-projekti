using UnityEngine;

public class Enemy : Humanoid
{
    Area _area;
    static int _count;

    protected override void Awake()
    { 
        base.Awake();
        _area = GameObject.FindWithTag("Area").GetComponent<Area>();
        _count++;
    }

    protected override void Die()
    {
        base.Die();
        _count--;
        if (_count == 0)
        {
            _area.EnemiesDead = true;
        }
    }
}
