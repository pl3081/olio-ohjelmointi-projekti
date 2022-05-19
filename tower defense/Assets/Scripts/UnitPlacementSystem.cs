using UnityEngine;

public class UnitPlacementSystem : MonoBehaviour
{
    UnitPlacer _placer;

    void Awake()
    {
        _placer = GetComponent<UnitPlacer>();
        _placer.enabled = false;
    }

    public void SetTarget(GameObject newTarget)
    {
        GameObject selectionPreview = _placer.Selection.Preview;
        if (selectionPreview && newTarget.name == $"{selectionPreview.name}(Clone)")
        {
            return;
        }
        _placer.enabled = true;
        _placer.SetTarget(newTarget);
    }

    public void Disable()
    {
        _placer.Selection.Clear();
        _placer.enabled = false;
    }
}