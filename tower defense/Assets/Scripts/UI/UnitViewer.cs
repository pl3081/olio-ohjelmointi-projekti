using UnityEngine;
using UnityEngine.UI;

public class UnitViewer : MonoBehaviour
{
    [SerializeField] Text unitName;
    [SerializeField] Text cost;
    [SerializeField] Text moneyText;
    Unit _target;
    Player _player;

    public Unit Target
    {
        get => _target;
        set
        {
            _target = value;
            unitName.text = value.name;
            cost.text = "Price: " + value.Cost;
        }
    }

    void UpdateMoneyText()
    {
        moneyText.text = "Money: " + Player.Instance.money;
    }

    void Start()
    {
        UpdateMoneyText();
        _player = Player.Instance;
    }
    
    public void Buy(Player.UnitContainer toBuy)
    {
        _player.BuyUnit(toBuy);
        UpdateMoneyText();
    }
}
