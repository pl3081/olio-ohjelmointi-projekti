using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitControls : MonoBehaviour
{
    List<Unit> ChosenUnits => Area.Units;
    private List<Vector3> _formation = new List<Vector3>();
    public GameObject unitParent;
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
        _formation.Clear();
        numInRow = Mathf.Min(numInRow, ChosenUnits.Count);
        int rows = Mathf.CeilToInt(ChosenUnits.Count / (float)numInRow);
        int count = 0;
        for(int row = 0; row < rows; row++)
        {
            if (row * numInRow + numInRow > ChosenUnits.Count)
                numInRow = ChosenUnits.Count - count;

            for (int i = 0; i < numInRow; i++)
            {
                if (count < ChosenUnits.Count)
                {
                    float xMul = i - (numInRow - 1) / 2f;
                    float yMul = (rows - 1) / 2f - row;
                    _formation.Add((Vector3.right * xMul + Vector3.forward * yMul) * distanceBetween);
                    count++;
                }
            }
        }
    }
    public void Deformate(Unit unit)
    {
        _formation[ChosenUnits.IndexOf(unit)] = Vector3.zero;
    }

    private Vector3 Rotated(Vector3 vector, Quaternion rotation, Vector3 pivot = default(Vector3))
    {
        return rotation * (vector - pivot) + pivot;
    }
    void AttackCommand(BasicUnit target)
    {
        foreach (Unit unit in ChosenUnits)
        {
            unit.SetAttackTarget(target);
            unit.AIController.SetBehaviour(Unit.AI.Behaviour.Aggressive);
        }
    }
    void MoveCommand(Vector3 position)
    {
        Vector3 dirToPoint = new Vector3();
        foreach (Unit unit in ChosenUnits)
        {
            dirToPoint += position - unit.transform.position;
        }
        for (int i = 0; i < ChosenUnits.Count; i++)
        {
            Vector3 formatedPosition = position + Rotated(_formation[i], Quaternion.LookRotation(dirToPoint));
            if (ChosenUnits[i].transform.GetComponent<NavMeshAgent>().enabled)
            {
                ChosenUnits[i].MoveTo(formatedPosition);
                ChosenUnits[i].AIController.SetBehaviour(Unit.AI.Behaviour.Defensive);
            }
        }
    }

    private void Awake()
    {
        player = Player.Instance;

        foreach(Player.UnitContainer container in player.units)
        {
            GameObject unitObject = container.unitObject;
            for (int i = 0; i < container.amount; i++)
            {
                GameObject newUnit = Instantiate(unitObject, new Vector3(i,0,i), Quaternion.identity);
                ChosenUnits.Add(newUnit.GetComponent<Unit>());
            }
        }

        Formate(3, 3f);
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
        }
        else if (Input.GetMouseButtonUp(0))
        {
            selBox.Destroy();
            selBox = null;
        }
    }
}