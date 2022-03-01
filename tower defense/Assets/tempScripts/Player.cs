using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] int money;
    [SerializeField] List<GameObject> buyableUnits;
    readonly Dictionary<GameObject, int> _units = new Dictionary<GameObject, int>();

    void Awake()
    {
        foreach (GameObject unit in buyableUnits)
        {
            _units[unit] = 0;
        }
    }
    
    public void BuyUnit(GameObject baseUnit)
    {
        var unit = baseUnit.GetComponent<Unit>();
        if (unit.Cost > money) return;
        money -= unit.Cost;
        _units[baseUnit] += 1;
    }

    public void DeployUnit(GameObject unit)
    {
        if (1 > _units[unit]) return;
        //add parameter for position
        _units[unit]--;
        Instantiate(unit, Vector3.zero, Quaternion.identity);
    }
}
