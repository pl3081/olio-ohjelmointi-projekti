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
        if (_placer.Preview && newTarget.name == $"{_placer.Preview.name}(Clone)")
        {
            return;
        }
        _placer.enabled = true;
        _placer.SetTarget(newTarget);
    }

    public void Disable()
    {
        _placer.Clear();
        _placer.enabled = false;
    }
}