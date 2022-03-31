using UnityEngine;
using UnityEngine.UI;

public class UnitViewer : MonoBehaviour
{
    [SerializeField] Text unitName;
    [SerializeField] Text cost;
    [SerializeField] Text moneyText;
    GameObject _target;

    public GameObject Target
    {
        get => _target;
        set
        {
            _target = value;
            Unit targetUnit = Target.GetComponent<Unit>();
            unitName.text = targetUnit.name;
            cost.text = "Price: " + targetUnit.Cost;
        }
    }

    void UpdateMoneyText()
    {
        moneyText.text = "Money: " + Player.Instance.money;
    }

    void Start()
    {
        UpdateMoneyText();
    }
    
    public void Buy()
    {
        if (!_target) return;
        Player.Instance.BuyUnit(_target);
        UpdateMoneyText();
    }
}
