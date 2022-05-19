using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitControls : MonoBehaviour
{
    public List<Unit> ControlledUnits => Area.Units;
    List<Unit> selectedUnits;

    public uint NumInFormationRow = 3;
    public float DistanceBetweenInFormation = 3f;
    List<Vector3> _formation = new List<Vector3>();

    Player player;

    SelectionBox selBox;
    [Serializable]
    public class SelectionSettings
    {
        public float Thickness;
        public Canvas Canvas;
        public Material Material;
        public Color Color;
    }
    public SelectionSettings selectionSettings;

    public void SetNewUnits(List<Unit> newUnits)
    {
        selectedUnits = new List<Unit>(newUnits);
        foreach(Unit unit in newUnits)
        {
            if (!ControlledUnits.Contains(unit))
            {
                selectedUnits.Remove(unit);
            }
        }
        Formate();
    }
    public bool AddUnit(Unit unit)
    {
        if (!IsUnitInList(unit))
        {
            selectedUnits.Add(unit);
            Formate();
            return true;
        }
        return true;
    }
    public bool RemoveUnit(Unit unit)
    {
        if (IsUnitInList(unit))
        {
            selectedUnits.Remove(unit);
            Formate();
            return true;
        }
        return false;
    }
    public void ClearUnits()
    {
        selectedUnits.Clear();
    }
    public bool IsUnitInList(Unit unit)
    {
        return selectedUnits.Contains(unit);
    }
    private bool CompareLists<T>(List<T> x, List<T> y)
    {
        if (x.Count != y.Count)
            return false;
        foreach(T element in x)
        {
            if (!y.Contains(element))
                return false;
        }
        return true;
    }
    private void Formate()
    {
        Formate((int)NumInFormationRow, DistanceBetweenInFormation);
    }
    public void Formate(int numInRow, float distanceBetween)
    {
        _formation.Clear();
        numInRow = Mathf.Min(numInRow, selectedUnits.Count);
        int rows = Mathf.CeilToInt(selectedUnits.Count / (float)numInRow);
        int count = 0;
        for(int row = 0; row < rows; row++)
        {
            if (row * numInRow + numInRow > selectedUnits.Count)
                numInRow = selectedUnits.Count - count;

            for (int i = 0; i < numInRow; i++)
            {
                if (count < selectedUnits.Count)
                {
                    float xMul = i - (numInRow - 1) * 0.5f;
                    float yMul = (rows - 1) * 0.5f - row;
                    _formation.Add((Vector3.right * xMul + Vector3.forward * yMul) * distanceBetween);
                    count++;
                }
            }
        }
    }

    private Vector3 Rotated(Vector3 vector, Quaternion rotation, Vector3 pivot = default(Vector3))
    {
        return rotation * (vector - pivot) + pivot;
    }
    void CheckUnits()
    {
        foreach (Unit unit in new List<Unit>(selectedUnits))
        {
            if (unit == null)
                selectedUnits.Remove(unit);
        }
    }
    void AttackCommand(BasicUnit target)
    {
        CheckUnits();
        foreach (Unit unit in selectedUnits)
        {
            unit.SetAttackTarget(target);
            unit.AIController.SetBehaviour(Unit.AI.Behaviour.Aggressive);
        }
    }
    void MoveCommand(Vector3 position)
    {
        CheckUnits();
        Vector3 dirToPoint = new Vector3();
        foreach (Unit unit in selectedUnits)
        {
            dirToPoint += position - unit.transform.position;
        }
        for (int i = 0; i < selectedUnits.Count; i++)
        {
            Vector3 formatedPosition = position + Rotated(_formation[i], Quaternion.LookRotation(dirToPoint));
            if (selectedUnits[i].transform.GetComponent<NavMeshAgent>().enabled)
            {
                selectedUnits[i].MoveTo(formatedPosition);
                selectedUnits[i].AIController.SetBehaviour(Unit.AI.Behaviour.Defensive);
            }
        }
    }

    private void Awake()
    {
        player = Player.Instance;

        selectedUnits = new List<Unit>();
        Formate();
    }
    
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if(hit.collider.CompareTag("Enemy"))
                {
                    AttackCommand(hit.transform.GetComponent<BasicUnit>());
                }
                else
                {
                    MoveCommand(hit.point);
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            selBox = new SelectionBox(Input.mousePosition, Input.mousePosition, selectionSettings.Canvas,
                selectionSettings.Thickness, selectionSettings.Color, selectionSettings.Material);
        }
        else if (Input.GetMouseButton(0))
        {
            selBox.EndPosition = Input.mousePosition;
            List<Unit> newUnits = selBox.GetObjectsUnderSelection<Unit>();
            if(!CompareLists(selectedUnits, newUnits))
                SetNewUnits(newUnits);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            selBox.Destroy();
            selBox = null;
        }
    }
}