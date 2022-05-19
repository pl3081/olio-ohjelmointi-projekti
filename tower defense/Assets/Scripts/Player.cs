using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int money;
    public static Player Instance { get; private set; }
    
    [Serializable] public class UnitContainer
    {
        public GameObject unitObject;
        public int amount;
    }
    public List<UnitContainer> units = new List<UnitContainer>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); //destroy testing player
        }
    }

    public UnitContainer FindContainer(string targetName)
    {
        return units.Find(container => container.unitObject.name == targetName);
    }
    
    public void BuyUnit(UnitContainer container)
    {
        var unit = container.unitObject.GetComponent<Unit>();
        if (unit.Cost > money) return;
        money -= unit.Cost;
        container.amount += 1;
    }
    
    public bool RemoveUnit(UnitContainer targetContainer)
    {
        if (1 > targetContainer.amount) return false;
        targetContainer.amount--;
        return true;
    }
}