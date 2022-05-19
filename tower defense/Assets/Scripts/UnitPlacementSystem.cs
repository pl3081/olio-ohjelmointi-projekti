using UnityEngine;

public class UnitPlacementSystem : MonoBehaviour
{
    UnitPlacer placer;

    void Awake()
    {
        placer = GetComponent<UnitPlacer>();
        placer.enabled = false;
    }

    public void SetTarget(GameObject newTarget)
    {
        GameObject selectionPreview = placer.Selection.Preview;
        if (selectionPreview && newTarget.name == $"{selectionPreview.name}(Clone)")
        {
            return;
        }
        placer.enabled = true;
        placer.SetTarget(newTarget);
    }

    public void Disable()
    {
        placer.Selection.Clear();
        placer.enabled = false;
    }
}