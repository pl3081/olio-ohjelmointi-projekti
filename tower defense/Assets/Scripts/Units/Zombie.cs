using System.Collections.Generic;
using UnityEngine;

public class Zombie : Unit
{
    ZombieAI _AIController;
    public override AI AIController => _AIController;
    protected override List<Unit> Enemies => Area.Units;

    public class ZombieAI : AI
    {
        Zombie _unit;
    
        public ZombieAI(Zombie unit) : base(unit)
        {
            behavPattern = Behaviour.Aggressive;
            _unit = unit;
        }
    }
    
    protected override void Awake()
    {
        base.Awake();
        _AIController = new ZombieAI(this);
        Area.Enemies.Add(this);
    }
    
    protected override void Update()
    {
        base.Update();
        _AIController.Update();
    }
}