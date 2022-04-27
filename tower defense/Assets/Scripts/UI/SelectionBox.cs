using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionBox
{
    Line[] lines = new Line[4]; // 0 - x1 x2, 1 - y1 y2, 2 - x1 y1, 3 x2 y2

    Vector3 startPosition;
    Vector3 endPosition;
    public Vector3 StartPosition
    {
        get => startPosition;
        set
        {
            startPosition = value;
            UpdatePositions(startPosition, endPosition);
        }
    }
    public Vector3 EndPosition
    {
        get => endPosition;
        set
        {
            endPosition = value;
            UpdatePositions(startPosition, endPosition);
        }
    }

    public SelectionBox(Vector3 startPos, Vector3 endPos,
        Canvas parentCanvas, float thickness, Color color, Material mat)
    {
        
        this.startPosition = startPos;
        this.endPosition = endPos;
        for(int i = 0; i < 4; i++)
        {
            if (i % 2 == 0)
                lines[i] = new Line(startPos, endPos, parentCanvas, thickness, color, mat, Line.LineDirection.Horizontal);
            else
                lines[i] = new Line(startPos, endPos, parentCanvas, thickness, color, mat, Line.LineDirection.Vertical);
        }
    }
    public List<T> GetObjectsUnderSelection<T>() where T : Component
    {
        Vector3[] vertices;
        vertices = new Vector3[]
        {
            ScreenToGround(startPosition),
            ScreenToGround(new Vector3(endPosition.x, startPosition.y)),
            ScreenToGround(endPosition),
            ScreenToGround(new Vector3(startPosition.x, endPosition.y))
        };

        float maxDistanceToVertices = GetMaxDistance(vertices);
        List<T> objects = new List<T>(Object.FindObjectsOfType<T>());
        Vector3 filter = Vector3.forward + Vector3.right;
        foreach (T obj in objects.ToArray())
        {
            Vector3 objPos = Vector3.Scale(obj.transform.position, filter);
            if (SumOfDistances(objPos, vertices) > maxDistanceToVertices)
            {
                objects.Remove(obj);
            }
        }
        
        return objects;
    }

    Vector3 ScreenToGround(Vector2 pos) // returns (x, 0, z)
    {
        Vector3 filter = Vector3.forward + Vector3.right;

        Ray ray = Camera.main.ScreenPointToRay(pos);
        
        RaycastHit hit;
        Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 3); // 3 - Ground

        return Vector3.Scale(hit.point, filter);
    }
    float GetMaxDistance(Vector3[] vertices)
    {
        return SumOfDistances(vertices[0], vertices);
    }
    float SumOfDistances(Vector3 pos, Vector3[] vertices) // todo make more precise algorithm
    {
        float sum = 0;
        foreach (Vector3 vertex in vertices)
        {
            sum += Vector3.Distance(pos, vertex);
        }
        return sum;
    }

    void UpdatePositions(Vector3 startPos, Vector3 endPos)
    {
        lines[0].ChangeLinePositions(startPos, endPos);
        lines[1].ChangeLinePositions(startPos, endPos);

        lines[2].ChangeLinePositions(new Vector3(startPos.x, endPos.y), new Vector3(endPos.x, endPos.y));
        lines[3].ChangeLinePositions(new Vector3(endPos.x, startPos.y), new Vector3(endPos.x, endPos.y));
    }

    public void Destroy()
    {
        foreach(Line line in lines)
        {
            line.Destroy();
        }
    }
}
