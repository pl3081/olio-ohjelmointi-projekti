using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Line
{
    public enum LineDirection
    {
        Horizontal,
        Vertical
    }
    LineDirection direction;
    public LineDirection Direction
    {
        get => direction;
        set
        {
            direction = value;
            if (direction == LineDirection.Horizontal)
            {
                filter = Vector3.right;
            }
            else
            {
                filter = Vector3.up;
            }
        }
    }
    Vector3 filter;

    Vector3 startPosition;
    Vector3 endPosition;
    public Vector3 StartPosition
    {
        get => startPosition;
        set
        {
            startPosition = value;
            UpdateLinePositions(startPosition, endPosition);

            bool isDefault = Vector2.Distance(lineRect.sizeDelta, new Vector2(Thickness, Thickness)) == 0;
            lineObject.SetActive(!isDefault);
        }
    }
    public Vector3 EndPosition
    {
        get => endPosition;
        set
        {
            endPosition = value;
            UpdateLinePositions(startPosition, endPosition);

            bool isDefault = Vector2.Distance(lineRect.sizeDelta, new Vector2(Thickness, Thickness)) == 0;
            lineObject.SetActive(!isDefault);
        }
    }

    GameObject lineObject;
    RectTransform lineRect;
    Image lineImage;

    Canvas parentCanvas;

    public Material Material;
    public float Thickness;
    public Color Color;

    public Line(Vector3 startPos, Vector3 endPos,
        Canvas parentCanvas, float lineThickness, Color color, Material mat,
        LineDirection direction = LineDirection.Horizontal)
    {

        this.Thickness = lineThickness;
        this.parentCanvas = parentCanvas;
        this.Color = color;
        this.Material = mat;

        this.Direction = direction;
        this.startPosition = startPos;
        this.endPosition = endPos;

        CreateLine();
    }
    public void ChangeLinePositions(Vector3 startPos, Vector3 endPos)
    {
        startPosition = startPos;
        endPosition = endPos;
        UpdateLinePositions(startPos, endPos);
        bool isDefault = Vector2.Distance(lineRect.sizeDelta, new Vector2(Thickness, Thickness)) == 0;
        lineObject.SetActive(!isDefault);
    }
    void CreateLine()
    {
        lineObject = new GameObject();
        lineObject.transform.parent = parentCanvas.transform;

        lineImage = lineObject.AddComponent<Image>();
        lineImage.material = Material;
        lineImage.color = Color;

        lineRect = lineObject.GetComponent<RectTransform>();
        lineRect.anchorMin = Vector2.zero;
        lineRect.anchorMax = Vector2.zero;
        lineRect.sizeDelta = new Vector2(Thickness, Thickness);
        UpdateLinePositions(StartPosition, EndPosition);
    }
    void UpdateLinePositions(Vector3 start, Vector3 end)
    {
        Vector3 change = end - start;
        Vector3 newPos = start + Vector3.Scale(change, filter) * 0.5f;

        lineRect.anchoredPosition = newPos;

        Vector2 Vector2Abs(Vector2 v) { return new Vector2(Mathf.Abs(v.x), Mathf.Abs(v.y)); }

        Vector2 newSize = Vector2Abs(Vector2.Scale(filter, change)) + new Vector2(Thickness, Thickness);
        Debug.Log(newSize);
        if (newSize.x < Thickness || newSize.y < Thickness)
            newSize = new Vector2(Thickness, Thickness);

        lineRect.sizeDelta = newSize;

        
    }
    public void Destroy()
    {
        GameObject.Destroy(lineObject);
    }
}
