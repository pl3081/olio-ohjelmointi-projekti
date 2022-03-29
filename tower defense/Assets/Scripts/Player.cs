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

    UnitContainer FindContainer(GameObject objectUnit)
    {
        return units.Find(container => container.unitObject == objectUnit);
    }
    
    public void BuyUnit(GameObject objectUnit)
    {
        var unit = objectUnit.GetComponent<Unit>();
        if (unit.Cost > money) return;
        money -= unit.Cost;
        UnitContainer targetContainer = FindContainer(objectUnit);
        targetContainer.amount += 1;
    }

    public void DeployUnit(GameObject objectUnit, Vector3 position)
    {
        UnitContainer targetContainer = FindContainer(objectUnit);
        if (1 > targetContainer.amount) return;
        targetContainer.amount--;
        Instantiate(objectUnit, position, Quaternion.identity);
    }
}