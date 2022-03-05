using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitControls : MonoBehaviour
{
    [SerializeField] private List<Unit> ChosenUnits;
    private List<Vector3> Formation = new List<Vector3>();

    public bool AddUnit(Unit unit)
    {
        if (!IsUnitInList(unit))
        {
            ChosenUnits.Add(unit);
            return true;
        }
        return true;
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
    public void Formate(int numInRow, float distanceBetween)
    {
        Formation.Clear();
        numInRow = Mathf.Min(numInRow, ChosenUnits.Count);
        int rows = Mathf.CeilToInt(ChosenUnits.Count / (float)numInRow);
        int count = 0;
        for(int row = 0; row < rows; row++)
        {
            if (row * numInRow > ChosenUnits.Count - count)
                numInRow = ChosenUnits.Count - count;

            for (int i = 0; i < numInRow; i++)
            {
                if (count < ChosenUnits.Count)
                {
                    float xMul = i - (numInRow - 1) / 2f;
                    float yMul = (rows - 1) / 2f - row;
                    Formation.Add((Vector3.right * xMul + Vector3.forward * yMul) * distanceBetween);
                    count++;
                }
            }
        }
    }
    public void Deformate(Unit unit)
    {
        Formation[ChosenUnits.IndexOf(unit)] = Vector3.zero;
    }

    private void Awake()
    {
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
                if(hit.collider.tag == "Enemy")
                {
                    foreach (Unit unit in ChosenUnits)
                    {
                        unit.SetAttackTarget(hit.transform.GetComponent<Humanoid>());
                    }
                }
                else
                {
                    for (int i = 0; i < ChosenUnits.Count; i++)
                    {
                        Quaternion direction = Quaternion.LookRotation((hit.point + Formation[i] - ChosenUnits[i].transform.position).normalized); 
                        Vector3 formatedPosition = hit.point + direction * Formation[i];
                        ChosenUnits[i].MoveTo(formatedPosition);
                    }
                }
            }
        }
    }
}