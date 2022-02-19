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
    private void Formate(int numInRow, float distanceBetween)
    {
        int rows = Mathf.CeilToInt(ChosenUnits.Count / (float)numInRow);
        int count = 0;
        for(int row = 0; row < rows; row++)
        {
            for(int i = 0; i < numInRow; i++)
            {
                if (count < ChosenUnits.Count)
                {
                    float xMul = i - (numInRow - 1) / 2f;
                    float yMul = (rows - 1) / 2f - row;
                    ChosenUnits[count].GroupPosOffset = (Vector3.right * xMul + Vector3.forward * yMul) * distanceBetween;
                    count++;
                }
            }
        }
    }
    private void Deformate(Unit unit)
    {
        unit.GroupPosOffset = Vector3.zero;
    }

    private void Awake()
    {
        if(ChosenUnits.Count > 1)
            Formate(2, 3f);
    }
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if(hit.collider.tag == "Humanoid")
                {
                    foreach (Unit unit in ChosenUnits)
                    {
                        unit.SetAttackTarget(hit.transform.GetComponent<Humanoid>());
                    }
                }
                else
                {
                    foreach (Unit unit in ChosenUnits)
                    {
                        unit.MoveTo(hit.point);
                    }
                }
            }
        }
    }
}
