using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitControls : MonoBehaviour
{
    [SerializeField] private List<Unit> ChosenUnits;

    public bool AddUnit(Unit unit)
    {
        if (!IsUnitInList(unit))
        {
            ChosenUnits.Add(unit);
            return true;
        }
        return false;
    }
    public bool RemoveUnit(Unit unit)
    {
        if (IsUnitInList(unit))
        {
            ChosenUnits.Remove(unit);
            return true;
        }
        return false;
    }
    public void ClearUnits()
    {
        ChosenUnits.Clear();
    }
    public bool IsUnitInList(Unit unit)
    {
        return ChosenUnits.Contains(unit);
    }
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                foreach (Unit unit in ChosenUnits)
                {
                    unit.MoveTo(hit.point);
                }
            }
        }
    }
}
