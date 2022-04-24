using UnityEngine;
using UnityEngine.UI;

public class UnitCard : MonoBehaviour
{
    UnitViewer _viewer;
    [SerializeField] Text nameText;
    GameObject _loadedUnit;

    void Awake()
    {
        _viewer = GameObject.Find("UnitViewer").GetComponent<UnitViewer>();
    }
    
    public void Load(GameObject unitObject)
    {
        _loadedUnit = unitObject;
        nameText.text = unitObject.name;
    }

    public void SetTarget()
    {
        _viewer.Target = _loadedUnit;
    }

    public void DirectBuy() => _viewer.Buy();
}
