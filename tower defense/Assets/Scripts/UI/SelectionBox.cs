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
    void UpdatePositions(Vector3 startPos, Vector3 endPos)
    {
        if(lines[2] == null) Debug.Log("no");
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
