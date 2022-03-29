using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int money;
    
    [SerializeField] List<GameObject> buyableUnits;
    public List<GameObject> BuyableUnits => buyableUnits;
    public static Player Instance;
    
    public readonly Dictionary<GameObject, int> Units = new Dictionary<GameObject, int>();

    void Awake()
    {
        Instance = this;
        foreach (GameObject unit in buyableUnits)
        {
            Units[unit] = 0;
        }
        DontDestroyOnLoad(gameObject);
    }
    
    public void BuyUnit(GameObject baseUnit)
    {
        var unit = baseUnit.GetComponent<Unit>();
        if (unit.Cost > money) return;
        money -= unit.Cost;
        Units[baseUnit] += 1;
    }

    public void DeployUnit(GameObject unit, Vector3 position)
    {
        if (1 > Units[unit]) return;
        Units[unit]--;
        Instantiate(unit, position, Quaternion.identity);
    }
}